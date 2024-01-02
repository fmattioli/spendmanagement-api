using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Client.Configuration;
using SpendManagement.Client.SpendManagementReadModel;
using SpendManagement.Infra.CrossCutting.Conf;

namespace SpendManagement.Infra.CrossCutting.Extensions.Requests
{
    public static class ExternalRequestExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection, SpendManagementDomainSettings? spendManagementDomain)
        {
            if (spendManagementDomain is not null)
            {
                serviceCollection
                    .AddHeaderPropagation(o => o.Headers.Add("Authorization"))
                    .AddSingleton<IApiConfiguration>(_ => new ApiConfiguration(spendManagementDomain.Url, ApiVersion.V1))
                    .AddHttpClient<ISpendManagementReadModelClient, SpendManagementReadModelClient>(b => b.BaseAddress = new Uri(spendManagementDomain.Url!))
                    .AddHeaderPropagation(o => o.Headers.Add("Authorization"));
            }
            return serviceCollection;
        }
    }
}
