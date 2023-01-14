using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spents.IntegrationTests.Tests
{
    public class BaseTests<T> where T : class
    {
        private HttpClient _httpClient;

        public BaseTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        protected async Task<(string response, HttpStatusCode statusCode)> PostAsync(string url, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, httpContent);
            return new(await response.Content.ReadAsStringAsync(), response.StatusCode);
        }
    }
}
