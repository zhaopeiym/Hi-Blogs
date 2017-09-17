using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Infrastructure.OAuthClient
{
    public class TencentQQClient
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackUrl { get; set; }

        public TencentQQClient(string clientId,string clientSecret,string callbackUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            CallbackUrl = callbackUrl;
        }

        public string GetAuthorizeUrl()
        {
            return @"https://graph.qq.com/oauth/show?which=Login&display=pc&client_id="+ ClientId + 
                 "&redirect_uri="+ CallbackUrl + "&response_type=code&display=default%20&state=qq";
        }
    }
}
