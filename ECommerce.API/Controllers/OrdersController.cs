using AutoMapper;
using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Core.Dtos;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail= User.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.ShippingAddress, orderDto.DeliveryMethodId);

            if (order is null) return BadRequest(new ApiResponse(400));

            return Ok(order);
        }

        [HttpGet]
        [Cached(6000)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await orderService.GetOrdersForSpecificUserAsync(buyerEmail);

            return Ok(orders);
        }

        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Cached(6000)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUserAsync(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order= await orderService.GetOrderByIdForUserAsync(id, buyerEmail);

            if(order is null) return NotFound(new ApiResponse(404));

            return Ok(order);
        }

        [HttpGet("deliveryMethods")]
        [Cached(6000)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAll()
            => Ok(await orderService.GetDeliveryMethodsAsync());
    }
}
