using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using System.Reflection.Metadata;

namespace PetShop.Services.DogProductItemService
{
    public class DogProductItemService : IDogProductItemService
    {
        private readonly PetShopDbContext _context;
        internal DbSet<DogProductItem> _dbset;
        public DogProductItemService(PetShopDbContext context)
        {
            _context = context;
            this._dbset = _context.Set<DogProductItem>();
        }

       public async Task<DogProductItem> Add(DogProductItemDto dogProductItemDto)
        {
            DateTime  currentDateTime = DateTime.Now;
            var images=JsonConvert.SerializeObject(dogProductItemDto.Images);
            if (await _context.DogProductItem.AnyAsync(product => product.ItemName == dogProductItemDto.ItemName))
            {

                throw new Exception("Tên sản phẩm đã tồn tại");
            }
            var newDogProductItem = new DogProductItem()
            {
                ItemName = dogProductItemDto.ItemName,
                Price = dogProductItemDto.Price,
                Category = dogProductItemDto.Category,
                Description = dogProductItemDto.Description,
                Images = images,
                IsDeleted = false,
                Quantity = dogProductItemDto.Quantity,
                CreateAt = currentDateTime
            };
            await _context.AddAsync(newDogProductItem);
            await _context.SaveChangesAsync();
            return newDogProductItem;
        }
        public async Task<DogProductItem?> Update(int id, DogProductItemDto dogProductItemDto)
        {
            var images = JsonConvert.SerializeObject(dogProductItemDto.Images);
            var newDogProductItem = await _context.DogProductItem.SingleOrDefaultAsync(product => product.DogProductItemId == id);
            if (newDogProductItem != null)
            {
                DateTime currentDateTime = DateTime.Now;
                newDogProductItem.ItemName = dogProductItemDto.ItemName;
                newDogProductItem.Price = dogProductItemDto.Price;
                newDogProductItem.Category = dogProductItemDto.Category;
                newDogProductItem.Description = dogProductItemDto.Description;
                newDogProductItem.Images = images;
                newDogProductItem.IsDeleted = dogProductItemDto.IsDeleted;
                newDogProductItem.Quantity = dogProductItemDto.Quantity;
                newDogProductItem.UpdatedAt = currentDateTime;
                await _context.SaveChangesAsync();
                return newDogProductItem;
            }
            
            return null;

        }

        public async Task<IActionResult> Delete(int id)
        {
            var dogProductItem = _dbset.SingleOrDefault(product => product.DogProductItemId == id);
            if (dogProductItem != null)
            {
                dogProductItem.IsDeleted = true;
                _dbset.Remove(dogProductItem);
                _context.SaveChanges();
                return await GetAll();
            }
            return null;
        }

        public async Task<IActionResult> Get(int id)
        {
            var dogProductItem = await _dbset.SingleOrDefaultAsync(product => product.DogProductItemId == id);
            if (dogProductItem != null)
            {
                return (ResponseHelper.Ok(
                    new
                    {
                        dogProductItem.DogProductItemId,
                        dogProductItem.ItemName,
                        dogProductItem.Price,
                        dogProductItem.Category,
                        dogProductItem.Description,
                        Images = JsonConvert.DeserializeObject(dogProductItem.Images),
                        dogProductItem.Quantity,
                        dogProductItem.IsInStock,
                        dogProductItem.IsDeleted
                    }));
            }
            return null;
        }

        public async Task<IActionResult> GetAll()
        {
            var dogproducts = await _context.DogProductItem.Where(d => d.IsDeleted != true).ToListAsync();
            List<object> responselist = new List<object>();
            dogproducts.ForEach(dogProductItem =>
            {
                var images = JsonConvert.DeserializeObject<string[]>(dogProductItem.Images);
                object response = new
                {
                    dogProductItem.DogProductItemId,
                    dogProductItem.ItemName,
                    dogProductItem.Price,
                    dogProductItem.Category,
                    dogProductItem.Description,
                    Images = images,
                    dogProductItem.Quantity,
                    dogProductItem.IsInStock,
                    dogProductItem.IsDeleted
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }
    }
}
