using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace SpendManagement.Application.Extensions
{
    public static class JsonPatchExtension
    {
        public static Action<JsonPatchError> HandlePatchErrors(ValidationResult validationResult)
        {
            return error => validationResult.Errors.Add(
                new ValidationFailure(error.Operation.path, error.ErrorMessage)
                {
                    ErrorCode = "104"
                });
        }
    }
}
