using SpendManagement.Application.Commands.AddReceipt;

namespace SpendManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(AddReceiptCommand).Assembly));

            return services;
        }
    }
}
