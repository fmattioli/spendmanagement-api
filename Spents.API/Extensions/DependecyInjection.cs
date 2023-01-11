using MediatR;
using Spents.Application.Commands.AddSpent;

namespace Spents.API.Extensions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddDependecyInjection(this IServiceCollection services)
        {
            services.AddMediatR(typeof(AddSpentCommand));
            return services;
        }
    }
}
