using MediatR;
using Spents.Application.Commands.AddReceipt;

namespace Spents.API.Extensions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddDependecyInjection(this IServiceCollection services)
        {
            services.AddMediatR(typeof(AddReceiptCommand));
            return services;
        }
    }
}
