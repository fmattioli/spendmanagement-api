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
                configuration.Endpoint + $"/{configuration.Version}");
        }

        protected async Task<TResponse> GetByIdAsync<TResponse>(
            string path,
            Guid id)
           where TResponse : class
        {
            var uri = BuildUri(path, id);
            return await this._httpClient.GetFromJsonAsync<TResponse>(uri);
        }

        private Uri BuildUri(string path, Guid id)
        {
            var uriString = $"{baseUri.AbsoluteUri}/{path.TrimStart('/')}/{id}";
            return new Uri(uriString);
        }
    }
}
