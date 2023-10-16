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
        Task<IActionResult> UpdateDogItem(int id, DogItemUpdateRequest request);
        Task<IActionResult> DeleteDogItem(int id);
        Task<IActionResult> GetDogBySpecies(int specieid);
        Task<IActionResult> GetAllDogItemsAdmin();
        Task<IActionResult> GetDogItemAdmin(int id);
    }
}
