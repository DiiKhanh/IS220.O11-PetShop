using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Services.CheckoutService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;

        }

        [HttpPost("create")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] CheckoutDto request)
        {
            return await _checkoutService.Create(request);
        }

        [HttpGet("list/{user_id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetByUser([FromRoute] string user_id)
        {
            return await _checkoutService.GetByUser(user_id);
        }

        [HttpPost("send")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SendEmail([FromBody] EmailCheckoutDto request)
        {
            return await _checkoutService.SendEmailCheckout(request);
        }

        [HttpGet("detail/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return await _checkoutService.GetDetail(id);
        }

        [HttpPost("vnpay")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CheckoutVnpay([FromBody] CheckoutVnpayDto request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            return await _checkoutService.CheckoutVnpay(request, ip);
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            return await _checkoutService.GetAll();
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, InvoiceDto request)
        {
            return await _checkoutService.Update(id, request);
        }
    }
}
