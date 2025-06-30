using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrdersRepository : GenericRepository<Orders>, IOrdersRepository
    {

        private readonly AppDbContext dbContext;

        public OrdersRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserId(int userId)
        {
            return  await dbContext.Orders.Where(o => o.LocalUserId == userId)
                .ToListAsync();



            
        }
    }
}
