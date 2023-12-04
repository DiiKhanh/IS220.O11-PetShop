using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Services.CartService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("init")]
        public async Task<IActionResult> createCart(){ return await cartService.createCart();}

        [HttpGet("get")]
        public async Task<IActionResult> getCart() 
        { 
            var res = await cartService.getCart(); 
            if (res == null) return NotFound();
            else return Ok(res);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> clearCart() { return await cartService.clearCart(); }

        [HttpPost("addItem/{id}")]
        public async Task<IActionResult> addItemtoCart([FromRoute]int id, CartProductDto dto) { return await cartService.addItemtoCart(id, dto); }


        [HttpPost("addDog/{id}")]
        public async Task<IActionResult> addDogtoCart([FromRoute] int id) { return await cartService.addDogtoCart(id); }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id) { return await cartService.delete(id); }

        [HttpDelete("delete")]
        public async Task<IActionResult> deletelist(CartDto dto) { return await cartService.deletelist(dto); }


        [HttpPut("update")]
        public async Task<IActionResult> update(List<CartProductDto> dto) { return await cartService.update(dto); }
    }
}
