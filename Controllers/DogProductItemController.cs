using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.DTOs.Wrapper;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.DogItemService;
using PetShop.Services.DogProductItemService;
using PetShop.Services.UriService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogProductItemController : ControllerBase
    {
        private readonly IDogProductItemService _dogProductItemService;
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService uriService;

        public DogProductItemController(IDogProductItemService dogProductItemservice, PetShopDbContext context, IMapper mapper, IUriService uriService)
        {
            _dogProductItemService = dogProductItemservice;
            _context = context;
            _mapper = mapper;
            this.uriService = uriService;
        }
        [HttpGet("get-all-dog-product-item")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _dogProductItemService.GetAll();
                return ResponseHelper.Ok(results);
            }
            catch(Exception ex) 
            {
                return ResponseHelper.BadRequest(ex.Message);
            }
            
        }

        [HttpGet("product")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.DogProductItem
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var pagedDataDto = _mapper.Map<List<DogProductItemResponse>>(pagedData);
            var totalRecords = await _context.DogItem.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<DogProductItemResponse>(pagedDataDto, validFilter, totalRecords, uriService, route);
            return ResponseHelper.Ok(pagedReponse);
        } 

        [HttpGet("get-dog-product-item/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _dogProductItemService.Get(id);
                if (result == null)
                {
                    return ResponseHelper.NotFound();
                }
                return ResponseHelper.Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        [HttpPut("update-dog-product-item/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, DogProductItemDtoUpdate product)
        {
            try
            {
                var result = await _dogProductItemService.Update(id, product);
                if (result == null)
                {
                    return ResponseHelper.NotFound();
                }
                return ResponseHelper.Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("add-dog-product-item")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DogProductItemDto product)
        {
            try
            {
                return ResponseHelper.Ok(await _dogProductItemService.Add(product));

            }
            catch(Exception ex)
            {
                return ResponseHelper.Created(ex.Message);
            }
        }

        [HttpDelete("delete-dog-product-item/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dogProductItemService.Delete(id);
                if (result == null)
                {
                    return ResponseHelper.NotFound();
                }
                return ResponseHelper.Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
