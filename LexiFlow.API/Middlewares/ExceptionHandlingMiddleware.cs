using LexiFlow.BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace LexiFlow.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Unexpected Error";
            string message = "An unexpected error occurred.";

            if (ex is BaseException bex)
            {
                statusCode = bex.StatusCode;
                title = bex.Title;
                message = bex.Message;
            }
            else if (ex.GetType() == typeof(Exception))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                title = "Validation Error";
                message = ex.Message;
            }

            var errorResponse = new
            {
                status = statusCode,
                title = title,
                message = message,
                timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}