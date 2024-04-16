using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        //Function To Build Query Dynamic
        public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecifications<T> specs)
        {
            var Query = query;
            if(specs.Criteria!= null)
            {
                Query=Query.Where(specs.Criteria);
            }
            Query=specs.Includes.Aggregate(Query,(currentQuery,includeExpression)=>currentQuery.Include(includeExpression));
            return Query;
        }
    }
}
