using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var errors = actionContext.
                ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(x=>x.Key,
                x=>x.Value.Errors.Select(x=>x.ErrorMessage).ToArray());

            var problem = new ProblemDetails()
            {
                Title="Validation Errors",
                Detail="One Or More Errors Occurred",
                Status=StatusCodes.Status400BadRequest,
                Extensions = { { "Errors",errors} }
            };
            return new BadRequestObjectResult(problem);
        }
    }
}
