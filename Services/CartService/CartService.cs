using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.UserService;
using System;
using System.Threading.Tasks;

namespace PetShop.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService userService;
        private readonly PetShopDbContext _context;

        public CartService(IConfiguration configuration, PetShopDbContext context, IUserService userService)
        {
            _configuration = configuration;
            _context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> createCart()
        {
            var CId = await userService.loginUser();
            
            Cart cart = new Cart() { CartId = CId, Total = 0 };
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(cart);
            
        }

        public async Task<Cart> getCart()
        {
            var CId = await userService.loginUser();
            var findCart = await _context.Cart
                .Include(c => c.cartDetails) 
                .FirstOrDefaultAsync(c => c.CartId == CId);
            return findCart;
        }

        public async Task<IActionResult> addItemtoCart(int id, CartProductDto dto)
        {
            var findCart = await getCart();

            if (findCart == null)
            {
                await createCart();
                findCart = await getCart();
            }
            var item = await _context.DogProductItem.FirstOrDefaultAsync(p => p.DogProductItemId == id);
            //orderfunc set instock = false when quantity=0
            if ((item != null) && (item.IsDeleted==false) && (item.IsInStock!=false))
            {
                var cartDetail = findCart.cartDetails.FirstOrDefault(c => c.ProductItemId == id);

                if (cartDetail == null)
                {
                    CartDetail new_item = new CartDetail()
                    {
                        CartId = findCart.CartId,
                        ProductItemId = item.DogProductItemId,
                        Quantity = dto.Quantity,
                        Price = item.Price
                    };
                    _context.CartDetail.Add(new_item);
                }
                else
                {
                    cartDetail.Quantity += dto.Quantity;
                    cartDetail.Price += item.Price;
                    _context.CartDetail.Update(cartDetail);

                }
                await _context.SaveChangesAsync();

                findCart.Total += dto.Quantity * item.Price;
                _context.Cart.Update(findCart);
                await _context.SaveChangesAsync();
                return ResponseHelper.Ok(findCart);
            }
            else
            {
                return ResponseHelper.BadRequest("Item is not available right now");
            }
        }

        public async Task<IActionResult> addDogtoCart(int id)
        {
            var findCart = await getCart();

            if (findCart == null)
            {
                await createCart();
                findCart = await getCart();
            }

            var item = await _context.DogItem.FirstOrDefaultAsync(p => p.DogItemId == id);
            if ((item != null) && (item.IsDeleted == false) && (item.IsInStock != false))
            {
                var cartDetail = findCart.cartDetails.FirstOrDefault(c => c.DogItemId == id);
                if (cartDetail == null)
                {
                    CartDetail new_item = new CartDetail()
                    {
                        CartId = findCart.CartId,
                        DogItemId = item.DogItemId,
                        Price = item.Price
                    };
                    _context.CartDetail.Add(new_item);
                    findCart.Total += item.Price;
                    _context.Cart.Update(findCart);
                    await _context.SaveChangesAsync();
                    return ResponseHelper.Ok(findCart);

                }
                else return ResponseHelper.BadRequest("Item has already available in your cart");
            }
            else
            {
                return ResponseHelper.BadRequest("Item is not available right now");
            }
        }   

        public async Task<IActionResult> clearCart()
        {
            var CId = await userService.loginUser();
            var findCart = await getCart();
            if (findCart != null)
            {
                _context.CartDetail.RemoveRange(findCart.cartDetails);
                findCart.Total = 0;
                await _context.SaveChangesAsync();
            }
            return ResponseHelper.Ok(findCart);
        }

        public async Task<IActionResult> delete(int id)
        {
            var findCart = await getCart();
            var cartDetailToRemove = findCart.cartDetails.FirstOrDefault(cd => cd.CartDetailId == id);

            if (cartDetailToRemove != null)
            {
                findCart.cartDetails.Remove(cartDetailToRemove);
                await _context.SaveChangesAsync();
                await calTotal(findCart);
            }

            return ResponseHelper.Ok(findCart);
        }

        public async Task<IActionResult> deletelist(CartDto dto)
        {
            var findCart = await getCart();

            var cartDetailsToRemove = findCart.cartDetails
                .Where(cd => dto.cartDetailId.Contains(cd.CartDetailId))
                .ToList();

            if (cartDetailsToRemove.Any())
            {
                foreach (var index in cartDetailsToRemove)
                {
                    findCart.cartDetails.Remove(index);
                }
                _context.CartDetail.RemoveRange(cartDetailsToRemove);
            }
            await _context.SaveChangesAsync();
            await calTotal(findCart);

            return ResponseHelper.Ok(findCart);
        }

        public async Task<IActionResult> update(List<CartProductDto> dto)
        {
            var findCart = await getCart();

            foreach (var index in dto)
            {
                var update = findCart.cartDetails.FirstOrDefault(cd => cd.ProductItemId == index.DogProductItemId);
                if (update != null)
                {
                    update.Quantity = index.Quantity;
                    _context.Update(update);
                }
            }
            await _context.SaveChangesAsync();
            await calTotal(findCart); 
            return ResponseHelper.Ok(findCart);
        }

        private async Task<int> calTotal(Cart findCart)
        {
            findCart.Total = await Task.Run(() => findCart.cartDetails.Sum(cd => cd.Quantity * cd.Price));
            _context.Update(findCart);
            await _context.SaveChangesAsync();
            return (int) findCart.Total;
        }
    }
}

