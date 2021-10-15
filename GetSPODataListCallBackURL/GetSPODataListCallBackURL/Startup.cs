using GetSPODataListCallBackURL.Cores;
using GetSPODataListCallBackURL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetSPODataListCallBackURL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var aad = Configuration.GetSection("AzureActiveDirectory");
            services.Configure<AzureActiveDirectory>(aad);
            var spo = Configuration.GetSection("SharePointOnline");
            services.Configure<SharePointOnline>(spo);

            services.AddHttpClient<IAAD, AAD>((httpClient) =>
            {
                var _aad = aad.Get<AzureActiveDirectory>();
                InitHttpClient(httpClient, _aad.Url);

            });

            services.AddHttpClient<ISPO, SPO>((httpClient) =>
            {
                var baseURL = spo.Get<SharePointOnline>().Url;
                InitHttpClient(httpClient, baseURL);
            }).ConfigureHttpClient((isp, configClient) =>
            {
                InitHttpClientConfig(isp, configClient, CacheKeys.BearerToken);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #region private init
        private HttpClient InitHttpClient(HttpClient httpClient, string baseUrl)
        {
            if (httpClient == null)
                httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(baseUrl))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
            }
            return httpClient;
        }
        private void InitHttpClientConfig(IServiceProvider isp, HttpClient configClient, string cacheKey)
        {
            configClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            var _cache = isp.GetService<IMemoryCache>();
            if (_cache.TryGetValue(cacheKey, out string accessToken))
            {
                configClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            }
        }

        #endregion
    }
}
