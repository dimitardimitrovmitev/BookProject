using System.Net;
using System.Text.Json;

namespace BookProject.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = ex switch
            {
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Forbidden, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                InvalidOperationException => (HttpStatusCode.Conflict, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = context.Response.StatusCode,
                message,
                // Only include detail in development
                detail = _env.IsDevelopment() ? ex.ToString() : null
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}