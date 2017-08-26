using AngleSharp.Parser.Html;
using HiBlogs.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogsMigration
{
    public class BlogMigrationcs
    {
        /// <summary>
        /// 从博客园迁移某篇文章
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isShowHome">是否显示在首页</param>
        /// <param name="isShowMyHome">是否显示在个人首页</param>
        /// <param name="isTransfer">是否为转载文章</param>
        public async Task<MigrationBlogModel> CnblogMigrationToHiBlogAsync(string url, bool isShowHome, bool isShowMyHome = true, bool isTransfer = true)
        {
            Console.WriteLine("正在解析博客内容" + url);
            using (HttpClient http = new HttpClient())
            {
                List<string> hosts = new List<string>();
                var htmlString = await http.GetStringAsync(url);
                HtmlParser htmlParser = new HtmlParser();

                #region foreach 和 ForEach的区别 
                var imgs = htmlParser.Parse(htmlString)
                             .QuerySelectorAll("#main #cnblogs_post_body img")
                             .ToList();
                foreach (var item in imgs)
                {
                    if (item == null) continue;
                    var src = item.Attributes.FirstOrDefault(f => f.Name == "src")?.Value;
                    if (!src.Contains("http"))
                        src = "http:" + src;
                    Console.WriteLine("正在保存图片" + src);
                    await SaveImg(src);
                    Uri uri = new Uri(src);
                    if (!hosts.Contains(uri.Scheme + "://" + uri.Host))
                        hosts.Add(uri.Scheme + "://" + uri.Host);
                }
                #endregion

                #region 诡异 await 好像并没有等待 （这样的话hosts可能还没取到值就执行下面的代码逻辑了 ）
                //htmlParser.Parse(htmlString)
                //             .QuerySelectorAll("#main #cnblogs_post_body img")
                //             .ToList().ForEach(async t =>
                //             {
                //                 if (t == null) return;

                //                 var src = t.Attributes.FirstOrDefault(f => f.Name == "src")?.Value;
                //                 if (!src.Contains("http"))
                //                     src = "http:" + src;
                //                 await SaveImg(src);
                //                 Uri uri = new Uri(src);
                //                 if (!hosts.Contains(uri.Scheme + "://" + uri.Host))
                //                     hosts.Add(uri.Scheme + "://" + uri.Host);
                //             });
                #endregion

                var blog = htmlParser.Parse(htmlString)
                                     .QuerySelectorAll("#main")
                                     .Select(t => new MigrationBlogModel()
                                     {
                                         Title = t.QuerySelectorAll(".postTitle a").FirstOrDefault()?.TextContent,
                                         Content = t.QuerySelectorAll("#cnblogs_post_body")
                                                     .FirstOrDefault()?
                                                     .InnerHtml
                                                     .ReplaceAll(hosts.ToArray(), "/File/BlogImgs"),
                                         OldPublishTiem = t.QuerySelectorAll("#post-date").FirstOrDefault()?.TextContent,
                                         Url = url,
                                     }).FirstOrDefault();
                return blog;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> SaveImg(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.Contains("http"))
                return null;
            var str = new string[] { "com/" };
            var filePath = url.Split(str, StringSplitOptions.RemoveEmptyEntries)[1];
            var path = "C:/File/BlogImgs/" + filePath.Substring(0, filePath.LastIndexOf('/'));
            var fileName = filePath.Substring(filePath.LastIndexOf('/'));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fileSavePath = path + fileName;
            if (!File.Exists(fileSavePath))
            {
                using (HttpClient http = new HttpClient())
                {
                    var bytes = await http.GetByteArrayAsync(url);//发送请求 (链接是a标签提供的)  
                    lock (string.Intern(fileSavePath))//锁定同一文件
                    {
                        if (!File.Exists(fileSavePath))
                        {
                            using (FileStream fs = new FileStream(fileSavePath, FileMode.CreateNew, FileAccess.Write))//使用追加方式打开一个文件流
                            {
                                fs.Write(bytes, 0, bytes.Length);//写入文件                   
                            }
                        }
                    }
                }
            }
            return filePath;
        }

        /// <summary>
        /// 从博客园迁移用户文章
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isShowHome"></param>
        /// <param name="isShowMyHome"></param>
        /// <param name="isTransfer"></param>
        /// <returns></returns>
        public async Task<List<MigrationBlogModel>> CnblogsMigrationToHiBlogAsync(string userName, bool isShowHome, bool isShowMyHome = true, bool isTransfer = true)
        {
            string url = "http://www.cnblogs.com/" + userName + @"/mvc/blog/sidecolumn.aspx";
            using (HttpClient http = new HttpClient())
            {
                var htmlString = await http.GetStringAsync(url);
                HtmlParser htmlParser = new HtmlParser();
                //档案url
                var archivesUrls = htmlParser.Parse(htmlString)
                                     .QuerySelectorAll(".catListPostArchive ul li")
                                     .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => f.Name == "href")?.Value)
                                     .ToList();

                //用户所有文章url
                var blogUrls = new List<string>();
                foreach (var archivesUrl in archivesUrls)
                {
                    htmlString = await http.GetStringAsync(archivesUrl);
                    var urls = htmlParser.Parse(htmlString)
                                          .QuerySelectorAll(".entrylist .entrylistPosttitle")
                                          .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => f.Name == "href")?.Value)
                                          .ToList();
                    blogUrls.AddRange(urls);
                }

                //用户所有文章
                var migrationBlogModels = new List<MigrationBlogModel>();
                foreach (var blogUrl in blogUrls)
                {
                    var blog = await CnblogMigrationToHiBlogAsync(blogUrl, isShowHome);
                    if (blog != null)
                        migrationBlogModels.Add(blog);
                }
                return migrationBlogModels;
            }
        }
    }
}
