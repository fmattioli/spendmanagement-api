using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Application.Commands.Validators;
using SpendManagement.Application.Validators;

namespace SpendManagement.Infra.CrossCutting.Extensions.Validators
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<JsonPatchError>, JsonPatchValidator>();
            services.AddScoped<IValidator<ReceiptInputModel>, AddReceiptValidator>();
            services.AddScoped<IValidator<RecurringReceiptInputModel>, AddRecurringReceiptValidator>();
            services.AddScoped<IValidator<CategoryInputModel>, AddCategoryValidator>();
            return services;
        }
    }
}
