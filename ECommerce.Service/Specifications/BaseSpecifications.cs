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

        public Expression<Func<Tentity, bool>> Criteria { get; }

        public Expression<Func<Tentity, object>>? OrderBy { private set; get; }
        protected void AddInclude(Expression<Func<Tentity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<Tentity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesc(Expression<Func<Tentity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }

        public Expression<Func<Tentity, object>>? OrderByDesc { private set; get; }

        protected BaseSpecifications(Expression<Func<Tentity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public int Skip { private set; get; }

        public int Take { private set; get; }

        public bool IsPaginated { private set; get; }


        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Skip = (pageSize - 1) * pageSize;
            Take = pageIndex;
        }
        // 12 Products => page size = 3 ,
        // skip = 6 and take = 3
        // skip =(page index -1)* page size = 2*3 = 6
        // take = page size = 3
    }
}
