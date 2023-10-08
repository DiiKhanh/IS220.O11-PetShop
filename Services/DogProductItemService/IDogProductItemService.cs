using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.DogProductItemService
{
    public interface IDogProductItemService
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> Get(int id);
        Task<DogProductItem> Add(DogProductItemDto dogProductItemDto);
        Task<DogProductItem?> Update(int id, DogProductItemDto dogProductItemDto);
        Task<IActionResult> Delete(int id);

    }
}
