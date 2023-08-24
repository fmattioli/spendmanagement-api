using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.Commands.Receipt.Services;

namespace SpendManagement.Infra.CrossCutting.Extensions.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IReceiptService, ReceiptService>();
            return serviceCollection;
        }
    }
}
