using ECommerce.Shared.CommonRespones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController:ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();

            else
                return HandleProblem(result.errors);

        }
        public ActionResult<TValue>HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProblem(result.errors);
        }


        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An Error Occurred"
                    );
            }

            if (errors.All(e => e.ErrorType == ErrorType.Validation))
            {
                return HandleValidationErrors(errors);
            }

            return HandleSingleError(errors[0]);

            
        }

        private ActionResult HandleSingleError(Error error)
        {
            return Problem
                (
                    title: error.Code,
                    detail: error.Description,
                    type: error.ErrorType.ToString(),
                    statusCode: MapErrorTypeIntoStatusCode(error.ErrorType)
                );
        }
        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors) {
                modelState.AddModelError(error.Code, error.Description);
             
            }
            return ValidationProblem(modelState);
        }

        private static int MapErrorTypeIntoStatusCode(ErrorType errorTybe) => errorTybe switch {
            ErrorType.NotFound =>StatusCodes.Status404NotFound,
            ErrorType.UnAuthorized =>StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden =>StatusCodes.Status403Forbidden,
            ErrorType.Validation =>StatusCodes.Status400BadRequest,
            ErrorType.InvalidCredintals =>StatusCodes.Status401Unauthorized,  // enum label
            _=>StatusCodes.Status500InternalServerError,
        };
    }
}
