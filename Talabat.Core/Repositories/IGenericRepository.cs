using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specification
        public Task<IReadOnlyList<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specification
        //Get All 
        public Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> Specs);

        // Get By Id 
        public Task<T> GetByIdWithSpecsAsync(ISpecifications<T> Specs);

        public Task<int> GetCountWithSpecsAsync(ISpecifications<T> specs);
    
        #endregion


    }
}
