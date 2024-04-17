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
            if(specs.Criteria!= null)  //Filteration
                Query=Query.Where(specs.Criteria);
            if(specs.OrderBy!= null)                       // ordering
                Query=Query.OrderBy(specs.OrderBy);
            if (specs.OrderByDescending != null)   // ordering
                Query = Query.OrderByDescending(specs.OrderByDescending);
            if (specs.IsPaginationEnabled)
                Query=Query.Skip(specs.Skip).Take(specs.Take); // Pagination
            Query=specs.Includes.Aggregate(Query,(currentQuery,includeExpression)=>currentQuery.Include(includeExpression)); // Includes
            return Query;
        }
    }
}
