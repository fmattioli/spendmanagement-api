using SpendManagement.Application.Commands.AddReceipt;
using SpendManagement.Application.Commands.UpdateReceipt;
using SpendManagement.Application.Commands.UpdateReceiptItem;

namespace SpendManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(AddReceiptCommand).Assembly,
                        typeof(UpdateReceiptCommand).Assembly,
                        typeof(UpdateReceiptItemCommand).Assembly));

            return services;
        }
    }
}
