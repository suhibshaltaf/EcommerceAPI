    using Ecommerce.Core.IRepositories;
    using Ecommerce.Infrastructure.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Ecommerce.Infrastructure.Repositories
    {
        public class UnitOfWork<T> : IUnitOfWork<T> where T : class
        {
            private readonly AppDbContext dbContext;

            public UnitOfWork(AppDbContext dbContext)
            {
                this.dbContext = dbContext;
            productsRepository = new ProductRepository(dbContext);
            OrdersRepository = new OrdersRepository(dbContext);

        }
            public IProductsRepositories productsRepository { get ; set;}

            public ICategoryRepository categoryRepository{ get; set;}
        public IOrdersRepository OrdersRepository { get; set; }

        public async Task< int> save()
                =>
                await dbContext.SaveChangesAsync();   
        }
    }
