using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
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
            using (HttpClient http = new HttpClient())
            {
                var htmlString = await http.GetStringAsync(url);
                HtmlParser htmlParser = new HtmlParser();
                var blog = htmlParser.Parse(htmlString)
                                     .QuerySelectorAll("#main")
                                     .Select(t => new MigrationBlogModel()
                                     {
                                         Title = t.QuerySelectorAll(".postTitle a").FirstOrDefault()?.TextContent,
                                         Content = t.QuerySelectorAll("#cnblogs_post_body").FirstOrDefault()?.InnerHtml,
                                         Url = url
                                     }).FirstOrDefault();
                return blog;
            }
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
                    migrationBlogModels.Add(await CnblogMigrationToHiBlogAsync(blogUrl, isShowHome));
                }
                return migrationBlogModels;
            }
        }
    }
}
