using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment environment)
        {
            _next = Next;
            _logger = logger;
            _environment = environment;
        }
        //Invoke Async
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                //if (_environment.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse(500);
                //}
                var Response= _environment.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse(500);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var jsonResponse=JsonSerializer.Serialize(Response,options);
                context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
