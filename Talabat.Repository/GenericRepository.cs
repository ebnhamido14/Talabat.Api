﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        #region Without Specification
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>)await _context.Products.Include(b => b.ProductBrand).Include(t => t.ProductType).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

      
        #endregion

        #region With Specification
        //Get All
        public async Task<IEnumerable<T>> GetAllWithSpecsAsync(ISpecifications<T> Specs)
        {
            return await ApplySpecifications(Specs).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecsAsync(ISpecifications<T> Specs)
        {
            return await ApplySpecifications(Specs).FirstOrDefaultAsync();
        }
        #endregion

        private IQueryable<T> ApplySpecifications(ISpecifications<T> Specs)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), Specs);
        }

    }
}
