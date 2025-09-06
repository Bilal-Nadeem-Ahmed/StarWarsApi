using static MyStarwarsApi.Exceptions.Exceptions;
using System.Net;
using System.Text.Json;

namespace MyStarwarsApi.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning(ex, "API Exception caught");
                await HandleExceptionAsync(context, ex.StatusCode, ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught");
                await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError, "An unexpected error occurred.", "SERVER_ERROR");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message, string errorCode)
        {
            var response = new
            {
                error = new
                {
                    code = errorCode,
                    message,
                    status = statusCode
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
