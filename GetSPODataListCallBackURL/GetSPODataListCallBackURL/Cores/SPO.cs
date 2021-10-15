using GetSPODataListCallBackURL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace GetSPODataListCallBackURL.Cores
{
    public interface ISPO
    {
        Task<IList<Country>> GetCountry(string listName);
    }

    public class SPO : ISPO
    {
        private readonly HttpClient _httpClient;

        public SPO(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<IList<Country>> GetCountry(string listName)
        {
            PropertyInfo[] propertyInfos = typeof(Country).GetProperties();
            Array.Sort(propertyInfos,
                    delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                    { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

            var dataColumn = string.Join(',', propertyInfos.Select(c => c.Name));
            if (string.IsNullOrEmpty(dataColumn))
                dataColumn = "*";
            string dataAPIAllData = $"lists/getbytitle('{listName}')/items?$top=10&$select={dataColumn }&$orderby=Modified desc";

            var countries = new List<Country>();
            var httpResponseMessage = await _httpClient.GetAsync(dataAPIAllData);
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!String.IsNullOrEmpty(response))
            {
                JObject jobj = JObject.Parse(response);
                JArray jarr = (JArray)jobj["d"]["results"];

                //Write Response to Output  
                foreach (JObject j in jarr)
                {
                    Country data = new Country();
                    data.Title = Convert.ToString(j["Title"]);
                    data.Name = Convert.ToString(j["Name"]);
                    data.Description = Convert.ToString(j["Description"]);

                    countries.Add(data);
                }
            }

            return countries;
        }
    }
}
