using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonRespones
{
    public class Error
    {
        private Error(string code, string description, ErrorType errorType) //can't create object now from ctor
        {
            Code = code;
            Description = description;  
            ErrorType = errorType;
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        public static Error Failure(string code = "General.Failure", string description = "A General Failure Has Occurred")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "General.Validation", string description = "A General Validation Has Occurred")
        {
            return new Error(code , description , ErrorType.Validation);
        }
        public static Error NotFound(string code = "NotFound.Failure", string description = "A General NotFound Has Occurred")
        {
            return new Error(code , description , ErrorType.NotFound);
        }

        public static Error UnAuthorized(
            string code = "Genaral.UnAuthorized",
            string description = "You are Not Authorized to perform this action"
        )
        {
            return new Error(code, description, ErrorType.UnAuthorized);
        }

        public static Error Forbidden(
            string code = "Genaral.Forbidden",
            string description = "You don't have the access to this resource,Access denied"
        )
        {
            return new Error(code, description, ErrorType.Forbidden);
        }

        public static Error InvalidCredintals(
            string code = "Genaral.InvalidCredintals",
            string description = "Your Credintals is not valid to reach this resource"
        )
        {
            return new Error(code, description, ErrorType.InvalidCredintals);
        }

    }
}
