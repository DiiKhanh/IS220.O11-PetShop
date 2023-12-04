using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.DTOs.Wrapper;
using PetShop.Helpers;
using PetShop.Models;
<<<<<<< HEAD
using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
=======
using PetShop.Services.UriService;
using SQLitePCL;
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50

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
            var dogitems = await _context.DogItem.Include(d => d.Species).Where(d => d.IsDeleted != true).ToListAsync();
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
        public async Task<IActionResult> AddDogItem(DogItemDto request)
        {
            var existsdog =
                await _context.DogItem.FirstOrDefaultAsync(e => e.DogName.ToLower().Trim() == request.DogName.ToLower().Trim());
            if (existsdog is not null) return ResponseHelper.BadRequest("Trùng tên chó rồi bạn ơi.");
            var specieid = _context.DogSpecies.FirstOrDefault(p => p.DogSpeciesName == request.SpeciesName);
            var dogmap = _mapper.Map<DogItem>(request);
            dogmap.DogSpeciesId = (int)specieid.DogSpeciesId;
            dogmap.Images = JsonConvert.SerializeObject(request.Images);
            dogmap.CreateAt = DateTime.UtcNow;
            //dogmap.UpdatedAt = DateTime.UtcNow;
            dogmap.IsDeleted = false;
            dogmap.IsInStock = true;
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
                //dogmap.UpdatedAt,
                dogmap.IsDeleted,
                dogmap.IsInStock
            });
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
                dogitem.UpdatedAt
            });
        }

<<<<<<< HEAD
        public async Task<IActionResult> UpdateDogItem(int id, DogItemUpdateRequest request)
        {
            var dogitem =
                await _context.DogItem.Include(d => d.Species).FirstOrDefaultAsync(x => x.DogItemId == id);
            _context.Entry(dogitem).State = EntityState.Modified;
            if (dogitem is null) return ResponseHelper.NotFound();
            var properties = typeof(DogItemUpdateRequest).GetProperties();
            try
            {
                foreach (var property in properties)
                {
                    var requestValue = property.GetValue(request);
                    if (requestValue is not null)
                    {
                        if (property.Name == "DogName")
                        {
                            var existsdog =
                            await _context.DogItem.FirstOrDefaultAsync(e => e.DogName.ToLower() == request.DogName.ToLower() && e.DogItemId != id);
                            if (existsdog is not null) return ResponseHelper.BadRequest("Trùng tên chó rồi bạn ơi.");
                            dogitem.DogName = request.DogName;
                        }
                        else if (property.Name == "SpeciesName")
                        {
                            var specieid = _context.DogSpecies.FirstOrDefault(p => p.DogSpeciesName == request.SpeciesName);
                            if (specieid is not null)
                            {
                                dogitem.DogSpeciesId = (int)specieid.DogSpeciesId;
                            }
                        }
                        else if (property.Name == "Images")
                        {
                            dogitem.Images = JsonConvert.SerializeObject(request.Images);
                        }
                        else
                        {
                            var dogitemProperty = typeof(DogItem).GetProperty(property.Name);
                            dogitemProperty.SetValue(dogitem, requestValue);
                        }
                    }
                }
            }
            catch
=======
        public async Task<IActionResult> UpdateDogItem(int id, DogItemDtoUpdate request)
        {
            var dogitem =
                await _context.DogItem
                .Include(c => c.Species)
                .FirstOrDefaultAsync(x => x.DogItemId == id);
            if (dogitem is null) return ResponseHelper.NotFound();
            if (request.DogName is not null)
            {
                var existsdog =
                    await _context.DogItem.FirstOrDefaultAsync(e => e.DogName.ToLower() == request.DogName.ToLower() && e.DogItemId != id);
                if (existsdog is not null) return ResponseHelper.BadRequest("Trùng tên chó rồi bạn ơi.");
            }
            try
            {
                if (request.SpeciesName is not null)
                {
                    var specieid = _context.DogSpecies.FirstOrDefault(p => p.DogSpeciesName == request.SpeciesName);
                    dogitem.DogSpeciesId = (int)specieid.DogSpeciesId;
                }
                if (request.Images is not null)
                {
                    var JsonImage = JsonConvert.SerializeObject(request.Images);
                    dogitem.Images = JsonImage;
                }
                dogitem.DogName = request.DogName ?? dogitem.DogName;
                dogitem.Price = request.Price ?? dogitem.Price;
                dogitem.Color = request.Color ?? dogitem.Color;
                dogitem.Sex = request.Sex ?? dogitem.Sex;
                dogitem.Age = request.Age ?? dogitem.Age;
                dogitem.Origin = request.Origin ?? dogitem.Origin;
                dogitem.HealthStatus = request.HealthStatus ?? dogitem.HealthStatus;
                dogitem.Description = request.Description ?? dogitem.Description;
                dogitem.UpdatedAt = DateTime.UtcNow;
                dogitem.IsDeleted = request.IsDeleted ?? dogitem.IsDeleted;
                dogitem.IsInStock = request.IsInStock ?? dogitem.IsInStock;
                await _context.SaveChangesAsync();
            } catch (Exception)
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
            {
                return ResponseHelper.BadRequest("Không thể cập nhật. Vui lòng thử lại");
            }
            dogitem.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
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
                dogitem.IsDeleted
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
                    dog.UpdatedAt
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
                    dog.IsDeleted
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
                dogitem.IsInStock
            });
        }

        
    }

    
}