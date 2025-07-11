using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
namespace StudentProgress.API.Middleware
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
                _logger.LogError(ex, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            if (exception is SqlException)
            {
                code = HttpStatusCode.ServiceUnavailable; // 503
                message = "Database connection error. Please try again later.";
            }
            else if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
                message = "Access denied.";
            }
            else if (exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            var errorResponse = new
            {
                error = new
                {
                    message = message,
                    details = exception.Message
                }
            };

            var result = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }

    }


}
