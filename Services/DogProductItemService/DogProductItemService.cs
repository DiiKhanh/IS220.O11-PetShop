using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Services.DogProductItemService
{
    public class DogProductItemService : IDogProductItemService
    {
        private readonly PetShopDbContext _context;
        internal DbSet<DogProductItem> _dbset;
        private readonly IWebHostEnvironment _environment;
        public DogProductItemService(PetShopDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            this._dbset = _context.Set<DogProductItem>();
            _environment = environment;
        }

       public async Task<DogProductItem> Add(DogProductItemDto dogProductItemDto)
        {
            DateTime  currentDateTime = DateTime.Now;
            string images=JsonConvert.SerializeObject(dogProductItemDto.Images);
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
            string images = JsonConvert.SerializeObject(dogProductItemDto.Images);
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

        public async Task<List<DogProductItem>?> Delete(int id)
        {
            var dogProductItem = _dbset.SingleOrDefault(product => product.DogProductItemId == id);
            if (dogProductItem != null)
            {
                dogProductItem.IsDeleted = true;
                _dbset.Remove(dogProductItem);
                _context.SaveChanges();
                return await _dbset.ToListAsync();
            }
            return null;
        }

        public async Task<DogProductItem?> Get(int id)
        {
            var dogProductItem = await _dbset.SingleOrDefaultAsync(product => product.DogProductItemId == id);
            if (dogProductItem != null)
            {
                return dogProductItem;
            }
            return null;
        }

        public async Task<List<DogProductItem>> GetAll()
        {
            return await _dbset.ToListAsync();
        }

       
        
    }
}
