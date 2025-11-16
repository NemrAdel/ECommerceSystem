using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.CustomeMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate next,ILogger<ExceptionHandlerMiddleWare> logger) // to know middleware have requestdelegate
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext) //must have Invoke / InvokeAsync To know this middleware
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex) 
            {
                //logging 
                _logger.LogError(ex,"Something Went Wrong."); // interface from microsoft 
                // my own return error response
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var problem = new ProblemDetails()
                {
                    Title="An unexpected error occured",
                    Status=StatusCodes.Status500InternalServerError,
                    Detail=ex.Message,
                    Instance=httpContext.Request.Path,
                };
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
