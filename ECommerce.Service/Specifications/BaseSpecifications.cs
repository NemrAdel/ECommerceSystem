using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications
{
    public abstract class BaseSpecifications<Tentity, Tkey> : ISpecifications<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<Tentity, object>>> IncludeExpressions { get; } = [];

        protected void AddInclude(Expression<Func<Tentity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

    }
}
