using Microsoft.Extensions.DependencyInjection;

using Spents.Core.Interfaces;
using Spents.Infra.Data.Persistence.Repositories;

namespace Spents.Infra.CrossCutting.Extensions.Repositories
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            return services;
        }
    }
}
