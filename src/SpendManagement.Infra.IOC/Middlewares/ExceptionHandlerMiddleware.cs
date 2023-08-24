using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using System.Net;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                JsonPatchInvalidException => HttpStatusCode.BadRequest,
                FluentValidation.ValidationException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError,
            };

            return (code, JsonConvert.SerializeObject(new
            {
                StatusCode = code,
                Errors = new List<string> { exception.Message },
            }));
        }
    }
}
