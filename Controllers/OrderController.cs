using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;
using PetShop.Services.OrderService;
using PetShop.Services.UriService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly IUriService uriService;

        public OrderController(IOrderService orderService, IConfiguration configuration, IUriService uriService)
        {
            _orderService = orderService;
            _configuration = configuration;
            this.uriService = uriService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> createOrder(ShipInfo request)
        {
            return await _orderService.createOrder(request);
        }

        [HttpGet("get-all-order")]
        public async Task<IActionResult> getAllOrder()
        {
            return await _orderService.getAllOrder();
        }

        [HttpGet("get-all-order-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllOrderAdmin()
        {
            return await _orderService.getAllOrderAdmin();
        }

        [HttpDelete("cancel-order/{oid}")]
        public async Task<IActionResult> cancelOrder([FromRoute] int oid)
        {
            return await _orderService.cancelOrder(oid);
        }

        [HttpGet("get-order/{oid}")]
        public async Task<IActionResult> getOrder([FromRoute] int oid)
        {
            return await _orderService.getOrder(oid);
        }

        [HttpGet("get-order-admin/{oid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getOrderAdmin([FromRoute] int oid)
        {
            return await _orderService.getOrderAdmin(oid);
        }

        [HttpPut("update-order/{oid}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> updateOrder([FromRoute] int oid, [FromBody] OrderDto request)
        {
            return await _orderService.updateOrder(oid, request);
        }

        [HttpPut("update-shipinfo/{sid}")]
        [Authorize]
        public async Task<IActionResult> updateShipInfo([FromRoute] int sid, [FromBody] UpdateShipInfoDto request)
        {
            return await _orderService.updateShipInfo(request,sid);
        }
    }
}
