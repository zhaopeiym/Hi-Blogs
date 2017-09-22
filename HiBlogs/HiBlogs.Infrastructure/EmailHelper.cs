using System;
using System.Net.Mail;
using System.Text;

namespace HiBlogs.Infrastructure
{
    public class EmailHelper
    {
        #region Eail 属性

        private string _mailFrom = "system@haojima.net";
        /// <summary>
        /// 发送者
        /// </summary>
        public string mailFrom { get { return _mailFrom; } set { _mailFrom = value; } }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] mailToArray { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] mailCcArray { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string mailSubject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string mailBody { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string mailPwd { get; set; }

        private string _host = "smtp.haojima.net";
        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string host { get { return _host; } set { _host = value; } }

        private bool _isbodyHtml = true;
        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool isbodyHtml { get { return _isbodyHtml; } set { _isbodyHtml = value; } }

        private string _nickname = "嗨-博客 系统通知";
        /// <summary>
        /// 发送者昵称
        /// </summary>
        public string nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
            }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] attachmentsPath { get; set; }

        //优先级别
        private MailPriority _Priority = MailPriority.Normal;
        /// <summary>
        /// 优先级别  默认正常优先级
        /// </summary>
        public MailPriority Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                _Priority = value;
            }
        }
        /// <summary>
        /// {0}:用户名
        /// {1}{2}{3}:正文内容
        /// </summary>
        public static string tempBody(string userName, string p1 = "", string p2 = "", string p3 = "", bool isShow = true)
        {
            return @"<STYLE type='text/css'>                                 
                                 BODY { font-size: 14px; line-height: 1.5  }   
                              </STYLE>
                       <HEAD>
                     <META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=UTF-8'> </HEAD>
                  <div style='background-color:#ECECEC; padding: 35px;'>
	                       <table cellpadding='0' align='center' style='width: 600px; margin: 0px auto; text-align: left; position: relative; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; font-size: 14px; font-family:微软雅黑, 黑体; line-height: 1.5; box-shadow: rgb(153, 153, 153) 0px 0px 5px; border-collapse: collapse; background-position: initial initial; background-repeat: initial initial;background:#fff;'>
		                <tbody>
			<tr>
				<th valign='middle' style='height: 25px; line-height: 25px; padding: 15px 35px; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #6f5499; background-color: #6f5499; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;'>
					<font face='微软雅黑' size='5' style='color: rgb(255, 255, 255); '>Hi-Blogs</font>
				</th>
			</tr>
			
			<tr>
				<td>
					<div style='padding:25px 35px 40px; background-color:#fff;'>
						<h2 style='margin: 5px 0px; '>
							<font color='#333333' style='line-height: 20px; '>
								<font style='line-height: 22px; ' size='4'>尊敬的 " + userName + @"，您好：</font>
							</font>
						</h2>
						<p>" + p1 + @"</p>
						<p>" + p2 + @"</p>
						<p>" + p3 + @"</p>
						<p>
							" + (isShow ? "如非本人操作，请不要理会此邮件，对此为您带来的不便深表歉意。" : string.Empty) + @"
						</p>
						<p>&nbsp;</p>
						<p align='right'>嗨-博客 官方团队</p>
						<p align='right'>" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"</p>
					</div>
				</td>
			</tr>
			
			<tr>
				<td>
					<div style='line-height: 20px;color: #999;background: #f5f5f5;font-size: 12px;border-top: 1px solid #ddd;padding: 10px 20px;'>        				
        				<p> 
        					如有疑问，请发邮件到 <a href='mailto:system@haojima.net' target='_blank'>system@haojima.net</a>，感谢您的支持。
        				</p>
    				</div>
				</td>
			</tr>
		</tbody>
	</table>
</div>
";
        }

        #endregion

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="CallSuccess">发送成功回调</param>
        /// <param name="CallFailure">发送失败回调</param>
        /// <returns></returns>
        public bool Send(Action<MailMessage> CallSuccess = null, Action<MailMessage> CallFailure = null)
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(mailFrom, nickname);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();

            //向收件人地址集合添加邮件地址
            if (mailToArray != null)
            {
                for (int i = 0; i < mailToArray.Length; i++)
                {
                    myMail.To.Add(mailToArray[i].ToString());
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (mailCcArray != null)
            {
                for (int i = 0; i < mailCcArray.Length; i++)
                {
                    myMail.CC.Add(mailCcArray[i].ToString());
                }
            }
            //发件人地址
            myMail.From = maddr;

            //电子邮件的标题
            myMail.Subject = mailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = mailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            //邮件优先级
            myMail.Priority = Priority;

            myMail.IsBodyHtml = isbodyHtml;


            //在有附件的情况下添加附件
            try
            {
                if (attachmentsPath != null && attachmentsPath.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in attachmentsPath)
                    {
                        attachFile = new Attachment(path);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }

            SmtpClient smtp = new SmtpClient();
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);//115                 //设置SMTP邮件服务器
            smtp.Host = host;
            smtp.Port = 80;
            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                if (CallSuccess != null)
                    CallSuccess(myMail);
                return true;

            }
            catch (SmtpException ex)
            {
                if (CallFailure != null)            
                     CallFailure(myMail);                
                return false;
            }

        }
    }
}
