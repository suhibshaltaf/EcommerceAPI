using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Products> unitOfWork;
        private readonly IMapper mapper;
        public ApiResponse response;

        public ProductController(IUnitOfWork<Products> unitOfWork,IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            response =new ApiResponse();
        }
        [HttpGet]
        [ResponseCache(CacheProfileName =("defaultCache"))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<ActionResult<ApiResponse>> GetAllProducts([FromQuery] string? categoryName=null, int pagesize=2, int pagenumber=1)
        {
            Expression<Func<Products,bool>> filter = null;
            if (!string.IsNullOrEmpty(categoryName))
            {
                filter = x=>x.Category.Name.Contains(categoryName);

            }
            var model = await unitOfWork.productsRepository.GetAll(includeProperty:"Category",page_size: pagesize, page_number :pagenumber, filter: filter);
            var check=model.Any();
            if (check)
            {
                response.StatusCode =200;
                response.IsSuccess= check;
                var mappedProducts=mapper.Map<IEnumerable<Products>,IEnumerable<ProductDTO>>(model);
                response.Result = mappedProducts    ;
                return response;

            }
            else {
                response.Messages = "not products found";
                response.IsSuccess= false;
                response.StatusCode=200;
                return response;
            }
            

        }
        [HttpGet("getById")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetById([FromQuery] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new APIValidationResponse(new List<string> { "Invalid Id", "Try Positive integer" }, 400));
                }

                var model = await unitOfWork.productsRepository.GetById(id);

                if (model == null)
                {
                    var x = model.ToString();
                    return NotFound(new ApiResponse(404, "product Not Found"));
                }
                return Ok(new ApiResponse(200, result: model));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIValidationResponse(new List<string> {  " internal server error",ex.Message}, StatusCodes.Status500InternalServerError));
            }
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateProduct( ProductCreateDTO productCreateDTO)
        {   
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.Messages = "Invalid data.";
                return BadRequest(response);
            }

            var product = mapper.Map<Products>(productCreateDTO);
            await unitOfWork.productsRepository.Create(product);
            await unitOfWork.save();

            response.IsSuccess = true;
            response.StatusCode = 200;
            response.Result = product;
            return Ok(response);

        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateProduct(Products model)
        {
            unitOfWork.productsRepository.Update(model);
           await unitOfWork.save();
            return Ok(model);


        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            unitOfWork.productsRepository.Delete(id);
            await unitOfWork.save();
            return Ok();


        }
        [HttpGet("Product/{cat_id}")]
        public async Task <ActionResult<ApiResponse>> GetAllProductsByCategoryId(int cat_id)
        {
            var Products=await unitOfWork.productsRepository.GetAllProductsByCategoryId(cat_id);
            var mappedProducts = mapper.Map<IEnumerable<Products>, IEnumerable<ProductDTO>>(Products);
            return Ok(mappedProducts);
        }
    }
}

