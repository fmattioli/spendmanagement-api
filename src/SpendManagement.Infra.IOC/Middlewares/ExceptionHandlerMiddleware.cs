using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError,
            };

            return (code, JsonConvert.SerializeObject(new Error
            {
                StatusCode = code,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            }));
        }
    }

    public record Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}
