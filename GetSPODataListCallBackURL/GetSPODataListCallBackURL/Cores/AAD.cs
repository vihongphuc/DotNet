using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GetSPODataListCallBackURL.Cores
{
    public interface IAAD
    {
        Task<string> GetAccessToken(string relativeUrl, StringContent strContent);
    }

    public class AAD : IAAD
    {
        private readonly HttpClient _httpClient;

        public AAD(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<string> GetAccessToken(string relativeUrl, StringContent strContent) {

            var result = await _httpClient.PostAsync(relativeUrl, strContent).ContinueWith((response) =>
            {
                return response.Result.Content.ReadAsStringAsync().Result;
            }).ConfigureAwait(false);

            var tokenResult = JsonSerializer.Deserialize<JsonElement>(result);
            var token = tokenResult.GetProperty("access_token").GetString();

            return token;
        }

    }
}
