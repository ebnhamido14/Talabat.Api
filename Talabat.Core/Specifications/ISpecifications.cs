using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; } //For where Condition

        public List<Expression<Func<T,object>>> Includes { get; set; } // For List Of Includes

        public Expression<Func<T,object>> OrderBy { get; set; } // for Order By 
        public Expression<Func<T,object>> OrderByDescending { get; set; } // for Order By Descending

        public int Take { get; set; } // for Take

        public int Skip { get; set; } // For Skip

        public bool IsPaginationEnabled { get; set; } // Check For Pagination
    }
}
