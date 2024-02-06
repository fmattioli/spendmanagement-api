using Flurl;
using SpendManagement.Client.Configuration;
using System.Net.Http.Json;

namespace SpendManagement.Client
{
    public class BaseClient(HttpClient httpClient, IApiConfiguration configuration)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly Uri baseUri = new(
                configuration.Endpoint + $"/{configuration.Version}");

        protected async Task<TResponse> GetAsync<TResponse>(
            string path,
            Guid id,
            string queryParamName)
           where TResponse : class
        {

            var uri = baseUri
                .AppendPathSegment(path)
                .AppendQueryParam(queryParamName, id);

            return await this._httpClient.GetFromJsonAsync<TResponse>(uri);
        }
    }
}
