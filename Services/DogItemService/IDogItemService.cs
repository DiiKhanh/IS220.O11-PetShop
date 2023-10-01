using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.DogItemService
{
    public interface IDogItemService
    {
        Task<IEnumerable<DogItem>> GetAllDogItems();
        Task<DogItem?> GetDogItem(int id);
        Task<DogItem> AddDogItem(DogItemDto request);
        Task<DogItem?> UpdateDogItem(int id, DogItemDto request);
        Task<IEnumerable<DogItem>?> DeleteDogItem(int id);
    }
}
