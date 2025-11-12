using ECommerce.Doamin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface ISpecifications<Tentitiy,Tkey>where Tentitiy : BaseEntity<Tkey>
    {
        ICollection<Expression<Func<Tentitiy,object>>> IncludeExpressions { get; } 

        Expression<Func<Tentitiy,bool>> Criteria { get; }

        Expression<Func<Tentitiy,object>>? OrderBy { get; }

        Expression<Func<Tentitiy,object>>? OrderByDesc { get; }
    }
}
