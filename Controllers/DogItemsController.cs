using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.DTOs.Wrapper;
using PetShop.Helpers;
using PetShop.Services.DogItemService;
using PetShop.Services.UriService;

namespace PetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogItemsController : ControllerBase
    {
        private readonly IDogItemService _dogitemservice;
        private readonly IMapper _mapper;
        private readonly IUriService uriService;
        private readonly PetShopDbContext _context;
        private readonly IDogSpeciesService _speciesservice;
        private readonly IConfiguration configuration;

        public DogItemsController(IDogItemService dogitemservice, IConfiguration _configuration, PetShopDbContext _context, IMapper mapper, IUriService uriService,
            IDogSpeciesService speciesservice)
        {
            _dogitemservice = dogitemservice;
            _mapper = mapper;
            this.uriService = uriService;
            this._context = _context;
            configuration = _configuration;
            _speciesservice = speciesservice;
        }

        //GET: api/DogItems/get-all-dog Lay danh sach chó
       [HttpGet]
       [Route("get-all")]
        public async Task<IActionResult> GetAllDogItem()
        {
            return await _dogitemservice.GetAllDogItems();
        }

        [HttpGet("dog")]        
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.DogItem
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var pagedDataDto = _mapper.Map<List<DogItemResponse>>(pagedData);
            var totalRecords = await _context.DogItem.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<DogItemResponse>(pagedDataDto, validFilter, totalRecords, uriService, route);
            return ResponseHelper.Ok(pagedReponse);
        }
        

        // GET: api/DogItems/get-dog/5 Lay thong tin chu cho có id = {id}
        [HttpGet("get-dog/{id}")]
        public async Task<IActionResult> GetDogItem([FromRoute]int id)
        {
            return await _dogitemservice.GetDogItem(id);
        }

        // PUT: api/DogItems/update-dog-item/5 update thông tin chú chó có id = {id}
        [HttpPut("update-dog-item/{id}")]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateDogItem([FromRoute]int id, DogItemDto request)
        //{
        //    return await _dogitemservice.UpdateDogItem(id, request);
        //}
        public async Task<IActionResult> UpdateDogItem([FromRoute] int id,[FromBody] DogItemUpdateRequest request)
        {
            return await _dogitemservice.UpdateDogItem(id, request);
        }

        // POST: api/DogItems/add-dog Thêm chó
        [HttpPost("add-dog")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDogItem([FromBody]DogItemDto request)
        {
            return await _dogitemservice.AddDogItem(request);
        }

        // DELETE: api/DogItems/5 Xóa 1 chú chó
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDogItem([FromRoute] int id)
        {
            return await _dogitemservice.DeleteDogItem(id);
        }

        //GET: api/DogItems/speciesid Lấy ra danh sách chó có id specie = {specieid}
        [HttpGet("get-dog-by-specie/{specieid}")]
        public async Task<IActionResult> GetDogItemBySpecie([FromRoute]int specieid)
        {
            return await _dogitemservice.GetDogBySpecies(specieid);
        }

        [HttpGet("get-all-species")]
        public async Task<IActionResult> GetAllSpecies()
        {
            return await _speciesservice.GetAllSpecies();
        }

        //GET: api/DogItems/get-all-dog Lay danh sach chó danh cho admin
        [HttpGet]
        [Route("get-all-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDogItemAdmin()
        {
            return await _dogitemservice.GetAllDogItemsAdmin();
        }

        // GET: api/DogItems/get-dog/5 Lay thong tin chu cho có id = {id}
        [HttpGet("get-dog-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDogItemAdmin([FromRoute] int id)
        {
            return await _dogitemservice.GetDogItemAdmin(id);
        }
    }
}
