using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Exceptions
{
    public class NotFoundExceptions(string message): Exception(message) // new constructor (primary constructor)
        // must have this ctor when make object and don't use another ctor
    {
        //public NotFoundExceptions(string message):base(message)
        //{
            
        //} // old constructor



    }
}
