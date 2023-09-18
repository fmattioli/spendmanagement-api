using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public abstract class AbstractExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public abstract (HttpStatusCode code, string message) GetResponse(Exception exception);

        protected AbstractExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                // log the error
                Log.Error(exception, "error during executing", context.Request.Path.Value);
                var response = context.Response;
                response.ContentType = "application/json";

                // get the response code and message
                var (status, message) = GetResponse(exception);
                response.StatusCode = (int)status;
                await response.WriteAsync(message);
            }
        }
    }
}
