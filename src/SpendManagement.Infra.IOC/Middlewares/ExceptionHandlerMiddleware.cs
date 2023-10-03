using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using System.Net;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger) : base(next)
        {
            _logger = logger;
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                JsonPatchInvalidException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                HttpRequestException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,
            };

            _logger.Error(
                    "The following error occurred ",
                    () => new
                    {
                        exception.Message,
                        ExceptionError = exception
                    });

            return (code, JsonConvert.SerializeObject(new
            {
                StatusCode = code,
                Errors = new List<string> { exception.Message },
            }));
        }
    }
}
