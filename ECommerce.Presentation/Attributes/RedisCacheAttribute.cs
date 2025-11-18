using ECommerce.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMinuites;

        public RedisCacheAttribute(int DurationInMinuites=5)
        {
            _durationInMinuites = DurationInMinuites;
        }
        override public void OnActionExecuting(ActionExecutingContext context)
        {

        }// before the action executed

        override public void OnActionExecuted(ActionExecutedContext context)
        {
        } // after the action executed


        override public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var cacheValue = await cacheService.GetAsync(cacheKey);
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                };
                return;
            }
            var ExecutedContext = await next.Invoke();
            if(ExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey,result.Value!,TimeSpan.FromMinutes(_durationInMinuites));
            }
        } // before and after the action executed


        //api , api/products , api/product?brandid=2 , ...............
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key= new StringBuilder();
            Key.Append($"{request.Path}"); // api/products base url
            foreach(var item in request.Query.OrderBy(x=>x.Key)) //sort it by key
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }
            return Key.ToString();
        }
    }
}
