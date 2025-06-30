using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IUnitOfWork<Categories> unitOfWork;

        public CategoriesController(AppDbContext dbContext, IUnitOfWork<Categories> unitOfWork)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
        }

    }
}
