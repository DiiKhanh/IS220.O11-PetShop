using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Services.DogItemService
{
    public class DogSpeciesService : IDogSpeciesService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;
        public DogSpeciesService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IActionResult> GetAllSpecies()
        //{
        //    return ResponseHelper.Ok(await _context.DogSpecies.ToListAsync());
        //}
        public async Task<IActionResult> GetAllSpecies()
        {
            var dogSpecies = await _context.DogSpecies.Include(ds => ds.dogItems).ToListAsync();
            return ResponseHelper.Ok(dogSpecies);
        }
    }
}
