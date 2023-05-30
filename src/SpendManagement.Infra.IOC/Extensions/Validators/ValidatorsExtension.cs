﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.InputModels.Common;
using SpendManagement.Application.InputModels.Validations;

namespace SpendManagement.Infra.CrossCutting.Extensions.Validators
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<ReceiptInputModel>, AddReceiptValidator>();
            return services;
        }
    }
}