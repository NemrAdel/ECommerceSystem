using ECommerce.Service.Exceptions;
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
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
                {
                    var problem = new ProblemDetails()
                    {
                        Title="Error While Processing Http Request. - EndPoint not found",
                        Status=StatusCodes.Status404NotFound,
                        Detail=$"Endpoint {httpContext.Request.Path} Not Found",
                        Instance=httpContext.Request.Path,
                    };

                    await httpContext.Response.WriteAsJsonAsync(problem);
                }

            }
            catch (Exception ex) 
            {
                //logging 
                _logger.LogError(ex,"Something Went Wrong.see here ---------------------------"); // interface from microsoft 
                // my own return error response
                //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; old way for all status code 500 but now we fix it
                var problem = new ProblemDetails()
                {
                    Title="An unexpected error occured",
                    Detail=ex.Message,
                    Instance=httpContext.Request.Path,
                    Status=ex switch
                    {
                        NotFoundExceptions=>StatusCodes.Status404NotFound,
                        _=>StatusCodes.Status500InternalServerError
                    }
                };
                httpContext.Response.StatusCode = problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
