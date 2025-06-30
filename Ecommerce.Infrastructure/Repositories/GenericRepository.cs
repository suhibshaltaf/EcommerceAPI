using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(T Model)
        {
           await  dbContext.Set<T>().AddAsync(Model);
        }

        public void Delete(int id)
        {
            dbContext.Remove(id);
        }

        public async Task< IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null , int page_size=2, int page_number=1, string? includeProperty = null)
        {

           
            IQueryable<T> query = dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperty != null)
            {
               
                    query = query.Include(includeProperty);

                
            }
            if (page_size > 0)
            {
                if (page_size > 4)
                {
                    page_size = 4;
                }
                query= query.Skip(page_size*(page_number-1)).Take(page_size);

            }
          return  await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return  await dbContext.Set<T>().FindAsync(id);
        }
            
        public void Update(T Model)
        {
            dbContext.Set<T>().Update(Model);
        }
    }
} 
