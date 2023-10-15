using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.CartService
{
    public interface ICartService
    {
        Task<IActionResult> createCart();
        Task<Cart> getCart();
        Task<IActionResult> addDogtoCart(int id);
        Task<IActionResult> addItemtoCart(int id, CartProductDto dto);
        Task<IActionResult> clearCart();
        Task<IActionResult> delete(int id);

        Task<IActionResult> deletelist(CartDto dto);
        Task<IActionResult> update(List<CartProductDto> dto);
    }
}
