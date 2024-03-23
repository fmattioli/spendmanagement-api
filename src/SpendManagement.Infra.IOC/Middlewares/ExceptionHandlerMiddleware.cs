using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SpendManagement.WebContracts.Exceptions;
using System.Net;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger) : AbstractExceptionHandlerMiddleware(next, logger)
    {
        private readonly ILogger _logger = logger;

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                InternalServerErrorException => HttpStatusCode.InternalServerError,
                ValidationException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                HttpRequestException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,
            };

            _logger.Error(exception, "The following error occurred ");

            return (code, JsonConvert.SerializeObject(new
            {
                StatusCode = code,
                Errors = new List<string> { exception.Message },
            }));
        }
    }
}
