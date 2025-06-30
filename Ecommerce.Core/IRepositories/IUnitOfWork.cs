using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.IRepositories
{
    public interface IUnitOfWork <T> where T : class
    {
        public IProductsRepositories  productsRepository { get; set; }
        public ICategoryRepository categoryRepository {  get; set; }
        IOrdersRepository OrdersRepository { get; set; }
        public Task< int >save();
    }
}
