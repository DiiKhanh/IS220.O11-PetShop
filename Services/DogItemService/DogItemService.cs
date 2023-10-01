using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.EmailService;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace PetShop.Services.DogItemService
{
    public class DogItemService : IDogItemService
    {
        private readonly PetShopDbContext _context;
        internal DbSet<DogItem> _dbset;
        public DogItemService(PetShopDbContext context)
        {
            _context = context;
            this._dbset = _context.Set<DogItem>();
        }

        public async Task<DogItem> AddDogItem(DogItemDto request)
        {
            var dogitem = new DogItem()
            {
                DogName = request.DogName,
                DogSpeciesId = request.DogSpeciesId,
                Price = request.Price,
                Color = request.Color,
                Sex = request.Sex,
                Age = request.Age,
                Origin = request.Origin,
                HealthStatus = request.HealthStatus,
                Description = request.Description,
                Images = request.Images,
            };
            await _dbset.AddAsync(dogitem);
            await _context.SaveChangesAsync();
            return dogitem;
        }

        public async Task<IEnumerable<DogItem>?> DeleteDogItem(int id)
        {
            var dogitem = _dbset.FirstOrDefault(x => x.DogItemId == id);
            if (dogitem == null) { return null; }
            //_dbset.Remove(dogitem);
            //_context.SaveChanges();
            dogitem.IsDeleted = true;
            return await _dbset.ToListAsync();
        }
        public async Task<IEnumerable<DogItem>> GetAllDogItems()
        {
            return await _dbset.ToListAsync();
        }
        public async Task<DogItem?> GetDogItem(int id)
        {
            var dogitem = await _dbset.FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null) return null;
            return dogitem;
        }

        public async Task<DogItem?> UpdateDogItem(int id, DogItemDto request)
        {
            var dogitem = await _dbset.FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null) return null;
            dogitem.DogName = request.DogName;
            dogitem.DogSpeciesId = request.DogSpeciesId;
            dogitem.Price = request.Price;
            dogitem.Color = request.Color;
            dogitem.Sex = request.Sex;
            dogitem.Age = request.Age;
            dogitem.Origin = request.Origin;
            dogitem.HealthStatus = request.HealthStatus;
            dogitem.Description = request.Description;
            dogitem.Images = request.Images;
            await _context.SaveChangesAsync();
            return dogitem;
        }
    }

}