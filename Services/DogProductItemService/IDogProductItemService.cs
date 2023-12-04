using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.DogProductItemService
{
    public interface IDogProductItemService
    {
        Task<List<DogProductItemResponse>> GetAll();
        Task<DogProductItemResponse> Get(int id);
        Task<DogProductItemResponse> Add(DogProductItemDto dogProductItemDto);
        Task<DogProductItemResponse?> Update(int id, DogProductItemDto dogProductItemDto);
        Task<List<DogProductItemResponse>> Delete(int id);
        Task<List<DogProductItemResponse>> GetAllAdmin();

    }
}
