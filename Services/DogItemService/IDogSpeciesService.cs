using Microsoft.AspNetCore.Mvc;

namespace PetShop.Services.DogItemService
{
    public interface IDogSpeciesService
    {
        Task<IActionResult> GetAllSpecies();
    }
}
