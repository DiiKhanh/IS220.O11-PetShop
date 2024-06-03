using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Services.DogItemService;

namespace PetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogItemsController : ControllerBase
    {
        private readonly IDogItemService _dogitemservice;
        private readonly IDogSpeciesService _speciesservice;
        private readonly IConfiguration configuration;

        public DogItemsController(IDogItemService dogitemservice, IConfiguration _configuration,
            IDogSpeciesService speciesservice)
        {
            _dogitemservice = dogitemservice;
            _configuration = configuration;
            _speciesservice = speciesservice;
        }

        //GET: api/DogItems/get-all-dog Lay danh sach chó
       [HttpGet]
       [Route("get-all")]
        public async Task<IActionResult> GetAllDogItem()
        {
            return await _dogitemservice.GetAllDogItems();
        }
        // Lay danh sach chó
        [HttpGet]
        [Route("get-all/{type}")]
        public async Task<IActionResult> GetAllDog([FromRoute] string type)
        {
            return await _dogitemservice.GetAllDog(type);
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
        public async Task<IActionResult> UpdateDogItem([FromRoute]int id, DogItemDto request)
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
