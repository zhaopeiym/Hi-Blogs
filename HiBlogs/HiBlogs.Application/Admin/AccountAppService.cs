using HiBlogs.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Talk.OAuthClient;

namespace HiBlogs.Application.Admin
{
    public class AccountAppService
    {
        public IOAuthClient GetOAuthClient(AuthType authType, HttpRequest Request)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string callbackUrl = string.Empty;

            if (authType == AuthType.QQ)
            {
                clientId = ConfigurationManager.GetSection("OAuthClient:TencentQQClient:ClientId");
                clientSecret = ConfigurationManager.GetSection("OAuthClient:TencentQQClient:ClientSecret");
                callbackUrl = "http://" + Request.Host.Value + ConfigurationManager.GetSection("OAuthClient:TencentQQClient:CallbackUrl");
            }
            else if (authType == AuthType.Sina)
            {
                clientId = ConfigurationManager.GetSection("OAuthClient:SinaClient:ClientId");
                clientSecret = ConfigurationManager.GetSection("OAuthClient:SinaClient:ClientSecret");
                callbackUrl = "http://" + Request.Host.Value + ConfigurationManager.GetSection("OAuthClient:SinaClient:CallbackUrl");
            }
            return OAuthClientFactory.GetOAuthClient(clientId, clientSecret, callbackUrl, authType);
        }
    }
}
