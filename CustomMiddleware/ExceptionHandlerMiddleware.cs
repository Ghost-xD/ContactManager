using Newtonsoft.Json;
using System.Net;

namespace ContactManager.CustomMiddleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e, ILogger<ExceptionHandlerMiddleware> logger)
        {
            var code = HttpStatusCode.InternalServerError;

            logger.LogError($"Unhandled Exception Occured! {code} - {e}");

            var result = JsonConvert.SerializeObject(new { error = e.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
