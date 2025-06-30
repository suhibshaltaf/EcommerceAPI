﻿using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter, int page_size, int page_number, string? includeProperty = null);
        public  Task <T> GetById(int id);
        public Task Create(T Model);
        public void Update(T  Model);
        public void Delete(int id);
    }
}
