using SpendManagement.Client.Configuration;
using System.Net.Http.Json;

namespace SpendManagement.Client
{
    public class BaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri baseUri;
        public BaseClient(HttpClient httpClient, IApiConfiguration configuration)
        {
            _httpClient = httpClient;
            this.baseUri = new Uri(
                new Uri(configuration.Endpoint),
                $"{configuration.Version}");
        }

        protected async Task<TResponse?> GetAsync<TResponse>(
            IDictionary<string, object> queryParams)
           where TResponse : class
        {
            var response = await this._httpClient.GetFromJsonAsync<TResponse>($"{baseUri.AbsoluteUri}/GetReceipt/{queryParams}");

            return response;
        }
    }
}
