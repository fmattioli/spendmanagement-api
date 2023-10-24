﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace SpendManagement.Application.Commands.Validators
{
    public class JsonPatchValidator : AbstractValidator<JsonPatchError>
    {
        public JsonPatchValidator()
        {
            RuleFor(x => x.ErrorMessage)
                .Empty();
        }

        protected override void RaiseValidationException(ValidationContext<JsonPatchError> context, ValidationResult result)
        {
            var ex = new ValidationException(string.Join(",", result.Errors.Select(x => x.AttemptedValue)));
            throw ex;
        }
    }
}
