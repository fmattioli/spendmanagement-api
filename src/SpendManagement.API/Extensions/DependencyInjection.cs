using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;

namespace SpendManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(AddReceiptCommand).Assembly,
                        typeof(UpdateReceiptCommand).Assembly));

            return services;
        }
    }
}
