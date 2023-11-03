using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace SpendManagement.Application.Extensions
{
    public static class JsonPatchExtension
    {
        public static Action<JsonPatchError> HandlePatchErrors(IValidator<JsonPatchError> validationResult)
        {
            return error => validationResult.ValidateAndThrow(error);
        }
    }
}
