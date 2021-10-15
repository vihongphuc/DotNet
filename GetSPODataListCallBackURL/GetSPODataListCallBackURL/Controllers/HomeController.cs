using GetSPODataListCallBackURL.Cores;
using GetSPODataListCallBackURL.Cores.Filter;
using GetSPODataListCallBackURL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace GetSPODataListCallBackURL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAAD _iADD;
        private readonly ISPO _iSPO;

        private readonly AzureActiveDirectory _aadModel;
        private readonly SharePointOnline _spoModel;


        public HomeController(ILogger<HomeController> logger,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor,

            IAAD iAAD,
            ISPO iSpoCode,

            IOptions<AzureActiveDirectory> aadModel,
            IOptions<SharePointOnline> spoModel)
        {
            _cache = cache;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            _iADD = iAAD;
            _iSPO = iSpoCode;

            _aadModel = aadModel.Value;
            _spoModel = spoModel.Value;
        }

        #region MVC Controller
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region AAD Callback

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">grand type is code</param>
        /// <param name="tenant">grand type is adminconsent</param>
        /// <param name="admin_consent">grand type is adminconsent</param>
        /// <param name="state">for both</param>
        /// <returns></returns>
        [Route("signin-oidc")]
        [QueryStringConstraintAttribute("code", true)]
        [QueryStringConstraintAttribute("state", true)]

        [QueryStringConstraintAttribute("tenant", false)]
        [QueryStringConstraintAttribute("admin_consent", false)]
        public IActionResult signin_oidc_code(string code, string state)
        {
            if (GlobalCommand.AAD_AUT_TYPE_CODE.Equals(state))
                _cache.Set(CacheKeys.Code, code, TimeSpan.FromDays(1));

            return RedirectToAction("Index");
        }


        [Route("signin-oidc")]
        [QueryStringConstraintAttribute("code", false)]
        [QueryStringConstraintAttribute("state", true)]

        [QueryStringConstraintAttribute("tenant", true)]
        [QueryStringConstraintAttribute("admin_consent", true)]
        public IActionResult signin_oidc_adminconsent(string tenant, bool admin_consent, string state)
        {
            if (GlobalCommand.AAD_AUT_TYPE_ADMIN_CONSENT.Equals(state))
                _cache.Set(CacheKeys.AdminConsent, tenant, TimeSpan.FromDays(1));

            return RedirectToAction("Index");
        }

        #endregion

        #region Ajax

        [HttpGet]
        public JsonResult AADAuthen(string authType)
        {
            var url = $"{_aadModel.Url}/{_aadModel.TanentId}/";
            var body0 = string.Empty;

            switch (authType)
            {
                case GlobalCommand.AAD_AUT_TYPE_CODE:
                    url += $"oauth2/v2.0/authorize?";

                    body0 = $"client_id={_aadModel.AppClientId}" +
                          $"&response_type=code" +
                          $"&redirect_uri= {GlobalCommand.GetFullUrl(_httpContextAccessor, _aadModel.CallBack)} " +
                          $"&response_mode=query" +
                        //$"&scope=openid offline_access https://graph.microsoft.com/.default" +
                        $"&scope={_spoModel.Scope}" +
                        $"&state={GlobalCommand.AAD_AUT_TYPE_CODE}";
                    //$"&code_challenge=3CB2B6D6B823E2C508CAD942245526287738D584E79FF87B4D354C7BD315F33" +
                    //$"&code_challenge_method=S256";
                    break;

                case GlobalCommand.AAD_AUT_TYPE_ADMIN_CONSENT:
                    url += $"adminconsent?";
                    body0 = $"client_id={_aadModel.AppClientId}" +
                                  $"&redirect_uri={GlobalCommand.GetFullUrl(_httpContextAccessor, _aadModel.CallBack)}" +
                                  $"&state={GlobalCommand.AAD_AUT_TYPE_ADMIN_CONSENT}";
                    break;

                default: break;
            }

            return new JsonResult(url + body0);
        }

        [HttpPost]
        public async Task<JsonResult> RequestAddToken(string authType)
        {
            var body = string.Empty;
            var relativeUrl = string.Format(GlobalCommand.AAD_TOKEN_V2, _aadModel.TanentId);
            switch (authType)
            {
                case GlobalCommand.AAD_AUT_TYPE_CODE:
                    if (_cache.TryGetValue(CacheKeys.Code, out string code))
                        body = $"client_id={_aadModel.AppClientId}&grant_type=authorization_code&client_secret={HttpUtility.UrlEncode(_aadModel.AppClientSecret)}&scope={HttpUtility.UrlEncode(_spoModel.Scope)}&code={code}&redirect_uri={GlobalCommand.GetFullUrl(_httpContextAccessor, _aadModel.CallBack)}&state={GlobalCommand.AAD_AUT_TYPE_CODE}";
                    break;

                case GlobalCommand.AAD_AUT_TYPE_ADMIN_CONSENT:
                    body = $"client_id={_aadModel.AppClientId}&grant_type=client_credentials&client_secret={HttpUtility.UrlEncode(_aadModel.AppClientSecret)}&scope={HttpUtility.UrlEncode(_spoModel.Scope)}";
                    break;

                case GlobalCommand.AAD_AUT_TYPE_PASSWORD:
                    relativeUrl = GlobalCommand.AAD_TOKEN_COMMON;
                    Uri resourceUri = new Uri(_spoModel.Url);
                    string resource = $"{resourceUri.Scheme}://{resourceUri.DnsSafeHost}";
                    body = $"resource={resource}&client_id={_aadModel.AppClientId}&grant_type=password&username={HttpUtility.UrlEncode(_aadModel.UserName)}&password={HttpUtility.UrlEncode(_aadModel.Password)}";
                    break;

                default:
                    break;
            }

            var accessToken = string.Empty;
            using (var stringContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"))
            {
                accessToken = await _iADD.GetAccessToken(relativeUrl, stringContent);
            }

            _cache.Set(CacheKeys.BearerToken, accessToken, TimeSpan.FromDays(1));

            return new JsonResult("{Result: true}");
        }

        [HttpPost]
        public async Task<JsonResult> GetSPOList(string authType)
        {
            var results = await _iSPO.GetCountry(_spoModel.DataListName);
            return new JsonResult(results);
        }

        #endregion

        #region Private

        public async Task<string> GetAPIResponseAsync(string url)
        {
            string response = String.Empty;
            try
            {
                string accessToken = string.Empty;
                if (_cache.TryGetValue(CacheKeys.BearerToken, out accessToken))
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                        var rs = httpClient.GetAsync(url);
                        response = await rs.Result.Content.ReadAsStringAsync();
                    }

                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
