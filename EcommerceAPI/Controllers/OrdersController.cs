using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork<Orders> unitOfWork;
        private readonly IMapper mapper;
        public ApiResponse response;

        public OrdersController(IUnitOfWork<Orders> unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            response = new ApiResponse();
        }
        
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetOrdersByUserId(int userId)
        {
            var orders = await unitOfWork.OrdersRepository.GetOrdersByUserId(userId);
            var check = orders.Any();
            if (check)
            {
                response.StatusCode = 200;
                response.IsSuccess = check;
                var mappedOrders = mapper.Map<IEnumerable<Orders>, IEnumerable<OrderDTO>>(orders);
                response.Result = mappedOrders;
                return response;
            }
            else
            {
                response.Messages = "No orders found for this user.";
                response.IsSuccess = false;
                response.StatusCode = 200;
                return response;
            }
        }
    }
}
