using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetSPODataListCallBackURL
{
    public static class GlobalCommand
    {
        public const string AAD_TOKEN_V2 = "{0}/oauth2/v2.0/token";
        public const string AAD_TOKEN_COMMON= "common/oauth2/token";

        public const string AAD_AUT_TYPE_CODE = "code";
        public const string AAD_AUT_TYPE_ADMIN_CONSENT = "adminconsent";
        public const string AAD_AUT_TYPE_PASSWORD = "password";

        public static string GetFullUrl(IHttpContextAccessor httpContextAccessor, string relativeUrl)
        {
            string host = httpContextAccessor.HttpContext.Request.Host.Value;
            host = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host + httpContextAccessor.HttpContext.Request.PathBase;
            var uri = new Uri(new Uri(host), relativeUrl);
            return uri.ToString();
        }
    }

    public static class CacheKeys
    {
        public static string BearerToken => "BearerToken";

        public static string Code => GlobalCommand.AAD_AUT_TYPE_CODE;
        public static string AdminConsent => GlobalCommand.AAD_AUT_TYPE_ADMIN_CONSENT;
    }
}
