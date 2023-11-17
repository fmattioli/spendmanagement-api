using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace SpendManagement.Client.Extensions
{
    public static class JsonPatchExtension
    {
        public static Action<JsonPatchError> HandlePatchErrors(IValidator<JsonPatchError> validationResult)
        {
            return error => validationResult.ValidateAndThrow(error);
        }
    }
}
