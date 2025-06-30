using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Products>,IProductsRepositories
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Products>> GetAllProductsByCategoryId(int Cat_Id)
        {
            //طرق للloading
            //1_eager loading
            /*
            var productss = (IEnumerable<Products>) await dbContext.Products.Include(x => x.Category).Where(c => c.Category_Id == Cat_Id).ToListAsync();
            return productss;*/
                //2_explicit loading 
               var products = await dbContext.Products.Where(c=>c.Category_Id==Cat_Id).ToListAsync();
        
            foreach (var product in products)
            {
                await dbContext.Entry(product).Reference(r=>r.Category).LoadAsync();
            }
            return products;
              //3_lazy loading 
              //هاي بدها مكتبه 
              /*var products =  await dbContext.Products.Where(c=>c.Category_Id==Cat_Id).ToListAsync();
            return products;*/
        }
    }
}
