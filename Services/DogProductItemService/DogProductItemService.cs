using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
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
        private readonly IMapper _mapper;
        public DogProductItemService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            this._dbset = _context.Set<DogProductItem>();
            this._mapper = mapper;
        }

       public async Task<DogProductItemResponse> Add(DogProductItemDto dogProductItemDto)
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
                Price = (int)dogProductItemDto.Price,
                Category = dogProductItemDto.Category,
                Description = dogProductItemDto.Description,
                Images = images,
                IsDeleted = false,
<<<<<<< HEAD
                Quantity = dogProductItemDto.Quantity,
                CreateAt = currentDateTime,
                UpdatedAt = currentDateTime,
                IsInStock = true
=======
                Quantity = (int)dogProductItemDto.Quantity,
                CreateAt = currentDateTime
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
            };
            DogProductItemResponse map = _mapper.Map<DogProductItemResponse>(newDogProductItem);
            map.Images = JsonConvert.DeserializeObject<string[]>(newDogProductItem.Images);
            await _context.AddAsync(newDogProductItem);
            await _context.SaveChangesAsync();
            return map;
        }
        public async Task<DogProductItemResponse?> Update(int id, DogProductItemDtoUpdate dogProductItemDto)
        {
            var images = JsonConvert.SerializeObject(dogProductItemDto.Images);
            var newDogProductItem = await _context.DogProductItem.SingleOrDefaultAsync(product => product.DogProductItemId == id);
            if (newDogProductItem != null)
            {
                DateTime currentDateTime = DateTime.Now;
<<<<<<< HEAD
                newDogProductItem.ItemName = dogProductItemDto.ItemName;
                newDogProductItem.Price = dogProductItemDto.Price;
                newDogProductItem.Category = dogProductItemDto.Category;
                newDogProductItem.Description = dogProductItemDto.Description;
                newDogProductItem.Images = images;
                newDogProductItem.IsDeleted = dogProductItemDto.IsDeleted;
                newDogProductItem.Quantity = dogProductItemDto.Quantity;
                newDogProductItem.UpdatedAt = currentDateTime;
                newDogProductItem.IsInStock = dogProductItemDto.IsInStock;
=======
                newDogProductItem.ItemName =     dogProductItemDto.ItemName    ?? newDogProductItem.ItemName     ;
                if (dogProductItemDto.Price.HasValue) newDogProductItem.Price = (int)dogProductItemDto.Price;
                newDogProductItem.Category =     dogProductItemDto.Category    ?? newDogProductItem.Category;
                newDogProductItem.Description =  dogProductItemDto.Description ?? newDogProductItem.Description;
                newDogProductItem.Images =       images                        ?? newDogProductItem.Images     ;
                newDogProductItem.IsDeleted =    dogProductItemDto.IsDeleted   ?? newDogProductItem.IsDeleted;
                if (dogProductItemDto.Quantity.HasValue) newDogProductItem.Quantity = (int)dogProductItemDto.Quantity;
                newDogProductItem.UpdatedAt =    currentDateTime;
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
                await _context.SaveChangesAsync();
                DogProductItemResponse response = _mapper.Map<DogProductItemResponse>(newDogProductItem);
                response.Images = JsonConvert.DeserializeObject<string[]>(newDogProductItem.Images);
                return response;
            }
            
            return null;

        }

        public async Task<List<DogProductItemResponse>> Delete(int id)
        {
            var dogProductItem = _dbset.SingleOrDefault(product => product.DogProductItemId == id);
            if (dogProductItem != null)
            {
                dogProductItem.IsDeleted = true;
                // _dbset.Remove(dogProductItem);
                await _context.SaveChangesAsync();
                return await GetAllAdmin();
            }
            return null;
        }

        public async Task<DogProductItemResponse> Get(int id)
        {
            var dogProductItem = await _dbset.SingleOrDefaultAsync(product => product.DogProductItemId == id);
            var dogproductmap = _mapper.Map<DogProductItemResponse>(dogProductItem);
            dogproductmap.DogProductItemId = (int)dogProductItem.DogProductItemId;
            dogproductmap.Images = JsonConvert.DeserializeObject<string[]>(dogProductItem.Images);
            if (dogProductItem != null)
            {
                return dogproductmap;
            }
            return null;
        }

        public async Task<List<DogProductItemResponse>> GetAll()
        {
            var items = await _context.DogProductItem.Where(d => d.IsDeleted != true).ToListAsync();
            List<DogProductItemResponse> responselist = new List<DogProductItemResponse>();
            items.ForEach(dogProductItem =>
            {
                DogProductItemResponse dogproducts = _mapper.Map<DogProductItemResponse>(dogProductItem);
                dogproducts.Images = JsonConvert.DeserializeObject<string[]>(dogProductItem.Images);

                responselist.Add(dogproducts);
            });
            return responselist;
        }

<<<<<<< HEAD
        public async Task<List<DogProductItemResponse>> GetAllAdmin()
        {
            var items = await _context.DogProductItem.ToListAsync();
            List<DogProductItemResponse> responselist = new List<DogProductItemResponse>();
            items.ForEach(dogProductItem =>
            {
                DogProductItemResponse dogproducts = _mapper.Map<DogProductItemResponse>(dogProductItem);
                dogproducts.Images = JsonConvert.DeserializeObject<string[]>(dogProductItem.Images);

                responselist.Add(dogproducts);
            });
            return responselist;
        }
=======
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
    }
}