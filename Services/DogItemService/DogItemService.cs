using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Services.DogItemService
{
    public class DogItemService : IDogItemService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;

        public DogItemService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllDogItems()
        {
            var dogitems = await _context.DogItem.Include(d => d.Species).Where(d => d.IsDeleted!=true).ToListAsync();
            List<object> responselist = new List<object>();
            dogitems.ForEach(dog =>
            {
                var images = JsonConvert.DeserializeObject<string[]>(dog.Images);
                object response = new
                {
                    dog.DogItemId,
                    dog.DogName,
                    dog.Species.DogSpeciesName,
                    dog.DogSpeciesId,
                    dog.Price,
                    dog.Color,
                    dog.Sex,
                    dog.Age,
                    dog.Origin,
                    dog.HealthStatus,
                    dog.Description,
                    Images = images,
                    dog.CreateAt,
                    dog.UpdatedAt,
                    dog.IsInStock,
                    dog.IsDeleted,
                    dog.Type
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }
        public async Task<IActionResult> AddDogItem(DogItemDto request)
        {
            var existsdog =
                await _context.DogItem.FirstOrDefaultAsync(e => e.DogName.ToLower().Trim() == request.DogName.ToLower().Trim());
            if (existsdog is not null) return ResponseHelper.BadRequest("Trùng tên thú cưng!");
            var specieid = _context.DogSpecies.FirstOrDefault(p => p.DogSpeciesName == request.SpeciesName);
            var dogmap = _mapper.Map<DogItem>(request);
            dogmap.DogSpeciesId = (int)specieid.DogSpeciesId;
            dogmap.Images = JsonConvert.SerializeObject(request.Images);
            dogmap.CreateAt = DateTime.UtcNow;
            dogmap.UpdatedAt = DateTime.UtcNow;
            dogmap.IsDeleted = false;
            dogmap.IsInStock = true;
            dogmap.Type = request.Type;
            await _context.DogItem.AddAsync(dogmap);
            await _context.SaveChangesAsync();
            var Images = JsonConvert.DeserializeObject<List<string>>(dogmap.Images);
            return ResponseHelper.Ok(new
            {
                dogmap.DogItemId,
                dogmap.DogName,
                dogmap.Species.DogSpeciesName,
                dogmap.Price,
                dogmap.Color,
                dogmap.Sex,
                dogmap.Age,
                dogmap.Origin,
                dogmap.HealthStatus,
                dogmap.Description,
                Images,
                dogmap.CreateAt,
                dogmap.UpdatedAt,
                dogmap.IsDeleted,
                dogmap.IsInStock,
                dogmap.Type
        }) ;
        }

        public async Task<IActionResult> DeleteDogItem(int id)
        {
            var dogitem = _context.DogItem.FirstOrDefault(x => x.DogItemId == id);
            if (dogitem is null || dogitem.IsDeleted == true) return ResponseHelper.NotFound();
            if (dogitem == null) return ResponseHelper.NotFound();
            //_context.DogItem.Remove(dogitem);
            dogitem.IsDeleted = true;
            await _context.SaveChangesAsync();
            return await GetAllDogItems();
        }

        public async Task<IActionResult> GetDogItem(int id)
        {
            var dogitem = await _context.DogItem.Include(d => d.Species).FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null || dogitem.IsDeleted == true) return ResponseHelper.NotFound();
            var Images = JsonConvert.DeserializeObject<List<string>>(dogitem.Images);
            return ResponseHelper.Ok(new
            {
                dogitem.DogItemId,
                dogitem.DogName,
                dogitem.Species.DogSpeciesName,
                dogitem.Price,
                dogitem.Color,
                dogitem.Sex,
                dogitem.Age,
                dogitem.Origin,
                dogitem.HealthStatus,
                dogitem.Description,
                Images,
                dogitem.CreateAt,
                dogitem.UpdatedAt,
                dogitem.IsInStock,
                dogitem.IsDeleted,
                dogitem.Type
            }) ;
        }

        public async Task<IActionResult> UpdateDogItem(int id, DogItemDto request)
        {
            var dogitem =
                await _context.DogItem.FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null) return ResponseHelper.NotFound();
            var existsdog =
                await _context.DogItem.FirstOrDefaultAsync(e => e.DogName.ToLower() == request.DogName.ToLower() && e.DogItemId != id);
            if (existsdog is not null) return ResponseHelper.BadRequest("Trùng tên chó rồi bạn ơi.");
            try
            {
                var specieid = _context.DogSpecies.FirstOrDefault(p => p.DogSpeciesName == request.SpeciesName);
                if (dogitem is null) return ResponseHelper.NotFound();
                _mapper.Map(request, dogitem);
                dogitem.Images = JsonConvert.SerializeObject(request.Images);
                dogitem.DogSpeciesId = (int)specieid.DogSpeciesId;
                dogitem.UpdatedAt = DateTime.UtcNow;
                dogitem.IsDeleted = request.IsDeleted;
                dogitem.IsInStock = request.IsInStock;
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                return ResponseHelper.BadRequest("Không thể cập nhật. Vui lòng thử lại");
            }
            var Images = JsonConvert.DeserializeObject<List<string>>(dogitem.Images);
            return ResponseHelper.Ok(new
            {
                dogitem.DogItemId,
                dogitem.DogName,
                dogitem.Species.DogSpeciesName,
                dogitem.Price,
                dogitem.Color,
                dogitem.Sex,
                dogitem.Age,
                dogitem.Origin,
                dogitem.HealthStatus,
                dogitem.Description,
                Images,
                dogitem.CreateAt,
                dogitem.UpdatedAt,
                dogitem.IsInStock,
                dogitem.IsDeleted,
                dogitem.Type
            }
            );
        }

        public async Task<IActionResult> GetDogBySpecies(int specieid)
        {
            var dogitems = await _context.DogItem.Include(d => d.Species).Where(e => e.DogSpeciesId == specieid).ToListAsync();
            List<object> responselist = new List<object>();
            dogitems.ForEach(dog =>
            {
                var images = JsonConvert.DeserializeObject<string[]>(dog.Images);
                object response = new
                {
                    dog.DogItemId,
                    dog.DogName,
                    dog.Species.DogSpeciesName,
                    dog.DogSpeciesId,
                    dog.Price,
                    dog.Color,
                    dog.Sex,
                    dog.Age,
                    dog.Origin,
                    dog.HealthStatus,
                    dog.Description,
                    Images = images,
                    dog.CreateAt,
                    dog.UpdatedAt,
                    dog.Type
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> GetAllDogItemsAdmin()
        {
            var dogitems = await _context.DogItem.Include(d => d.Species).ToListAsync();
            List<object> responselist = new List<object>();
            dogitems.ForEach(async dog =>
            {
                var images = JsonConvert.DeserializeObject<string[]>(dog.Images);
                if (dog.IsDeleted == null)
                {
                    dog.IsDeleted = false;
                    await _context.SaveChangesAsync();
                }
                if (dog.IsInStock == null)
                {
                    dog.IsInStock = true;
                    await _context.SaveChangesAsync();
                }
                
                object response = new
                {
                    dog.DogItemId,
                    dog.DogName,
                    dog.Species.DogSpeciesName,
                    dog.DogSpeciesId,
                    dog.Price,
                    dog.Color,
                    dog.Sex,
                    dog.Age,
                    dog.Origin,
                    dog.HealthStatus,
                    dog.Description,
                    Images = images,
                    dog.CreateAt,
                    dog.UpdatedAt,
                    dog.IsInStock,
                    dog.IsDeleted,
                    dog.Type
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> GetDogItemAdmin(int id)
        {
            var dogitem = await _context.DogItem.Include(d => d.Species).FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null) return ResponseHelper.NotFound();
            var Images = JsonConvert.DeserializeObject<List<string>>(dogitem.Images);
            return ResponseHelper.Ok(new
            {
                dogitem.DogItemId,
                dogitem.DogName,
                dogitem.Species.DogSpeciesName,
                dogitem.Price,
                dogitem.Color,
                dogitem.Sex,
                dogitem.Age,
                dogitem.Origin,
                dogitem.HealthStatus,
                dogitem.Description,
                Images,
                dogitem.CreateAt,
                dogitem.UpdatedAt,
                dogitem.IsDeleted,
                dogitem.IsInStock,
                dogitem.Type
            });
        }

        public async Task<IActionResult> GetAllDog(string type)
        {
            var dogitems = await _context.DogItem.Include(d => d.Species).Where(d => d.IsDeleted != true && type==d.Type).ToListAsync();
            List<object> responselist = new List<object>();
            dogitems.ForEach(dog =>
            {
                var images = JsonConvert.DeserializeObject<string[]>(dog.Images);
                object response = new
                {
                    dog.DogItemId,
                    dog.DogName,
                    dog.Species.DogSpeciesName,
                    dog.DogSpeciesId,
                    dog.Price,
                    dog.Color,
                    dog.Sex,
                    dog.Age,
                    dog.Origin,
                    dog.HealthStatus,
                    dog.Description,
                    Images = images,
                    dog.CreateAt,
                    dog.UpdatedAt,
                    dog.IsInStock,
                    dog.IsDeleted
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }
    }
}