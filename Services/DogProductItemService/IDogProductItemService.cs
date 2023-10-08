using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.DogProductItemService
{
    public interface IDogProductItemService
    {
        Task<List<DogProductItem>> GetAll();
        Task<DogProductItem?> Get(int id);
        Task<DogProductItem> Add(DogProductItemDto dogProductItemDto);
        Task<DogProductItem?> Update(int id, DogProductItemDto dogProductItemDto);
        Task<List<DogProductItem>?> Delete(int id);

    }
}
