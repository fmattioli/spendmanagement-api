using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Spents.Application.InputModels;
using Spents.Application.InputModels.Validations;

namespace Spents.Infra.CrossCutting.Extensions.Validators
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<AddReceiptInputModel>, AddReceiptValidator>();
            return services;
        }
    }
}
