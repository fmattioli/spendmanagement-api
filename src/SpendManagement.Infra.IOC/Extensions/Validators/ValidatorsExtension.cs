using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.Validations;

namespace SpendManagement.Infra.CrossCutting.Extensions.Validators
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<ReceiptInputModel>, AddReceiptValidator>();
            services.AddScoped<ValidationResult>();
            return services;
        }
    }
}
