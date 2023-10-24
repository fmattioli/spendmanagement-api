using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Validators;
using SpendManagement.Application.Validators;

namespace SpendManagement.Infra.CrossCutting.Extensions.Validators
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<JsonPatchError>, JsonPatchValidator>();
            services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<ReceiptInputModel>, AddReceiptValidator>();
            return services;
        }
    }
}
