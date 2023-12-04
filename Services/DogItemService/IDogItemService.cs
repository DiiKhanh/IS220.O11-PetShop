using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;
using System.Text.Json.Nodes;

namespace PetShop.Services.DogItemService
{
    public interface IDogItemService
    {
        Task<IActionResult> GetAllDogItems();
        Task<IActionResult> GetDogItem(int id);
        Task<IActionResult> AddDogItem(DogItemDto request);
<<<<<<< HEAD
        Task<IActionResult> UpdateDogItem(int id, DogItemUpdateRequest request);
=======
        Task<IActionResult> UpdateDogItem(int id, DogItemDtoUpdate request);
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
        Task<IActionResult> DeleteDogItem(int id);
        Task<IActionResult> GetDogBySpecies(int specieid);
        Task<IActionResult> GetAllDogItemsAdmin();
        Task<IActionResult> GetDogItemAdmin(int id);
    }
}
