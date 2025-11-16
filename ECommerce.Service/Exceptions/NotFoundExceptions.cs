using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Exceptions
{
    public abstract class NotFoundExceptions(string message): Exception(message) // new constructor (primary constructor)
        // must have this ctor when make object and don't use another ctor
        // will be immutable can't modify it because we don't have another ctor while this exist   (c# 12)
        // but have many problems in publishing on any server so we safely use classic ctor
    {
        //public NotFoundExceptions(string message):base(message)
        //{
            
        //} // old constructor



    }
    public sealed class ProductNotFoundException(int id ) 
        : NotFoundExceptions($"Product with id {id} is not found")
    {

    }
    public sealed class BasketNotFoundException(string id ) 
        : NotFoundExceptions($"Basket with id {id} is not found")
    {

    }
}
