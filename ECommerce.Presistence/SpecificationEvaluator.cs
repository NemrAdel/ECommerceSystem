using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<Tentity> CreateQuery<Tentity,Tkey>(IQueryable<Tentity> entryPoint
            ,ISpecifications<Tentity,Tkey> specifications)where Tentity : BaseEntity<Tkey>
        {
            var query = entryPoint;
            if ( specifications is not null)
            {
                if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    query = specifications.IncludeExpressions.Aggregate(query, (currentQuery,
                        includeExpression) =>currentQuery.Include(includeExpression));
                }
            }
            return query;
        }
    }
}
