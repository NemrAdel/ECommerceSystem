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

        public static Error Failure(string code = "General Failure", string description = "A General Failure Has Occurred")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "General Failure", string description = "A General Failure Has Occurred")
        {
            return new Error(code , description , ErrorType.Validation);
        }

    }
}
