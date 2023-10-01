using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public DogItemsController(IDogItemService dogitemservice)
        {
            _dogitemservice = dogitemservice;

        }

        //GET: api/DogItems/get-all-dog Lay danh sach chó
       [HttpGet]
       [Route("get-all")]
        public async Task<IActionResult> GetAllDogItem()
        {
            var results = await _dogitemservice.GetAllDogItems();
            return ResponseHelper.Ok(results);
        }

        // GET: api/DogItems/get-dog/5 Lay thong tin chu cho có id = {id}
        [HttpGet("get-dog/{id}")]
        public async Task<IActionResult> GetDogItem(int id)
        {
            var result = await _dogitemservice.GetDogItem(id);
            if (result == null)
            {
                return ResponseHelper.NotFound();
            }
            return ResponseHelper.Ok(result);
        }

        // PUT: api/DogItems/update-dog-item/5 update thông tin chú chó có id = {id}
        [HttpPut("update-dog-item/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateDogItem(int id, DogItemDto request)
        {
            var result = await _dogitemservice.UpdateDogItem(id, request);
            if (result == null)
            {
                return ResponseHelper.BadRequest("Không tìm thấy chú chó cần update.");
            }
            return ResponseHelper.Ok(result);
        }

        // POST: api/DogItems/add-dog Thêm chó
        [HttpPost("add-dog")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDogItem(DogItemDto request)
        {
            return ResponseHelper.Ok(await _dogitemservice.AddDogItem(request));
        }

        // DELETE: api/DogItems/5 Xóa 1 chú chó
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDogItem(int id)
        {
            var result = await _dogitemservice.DeleteDogItem(id);
            if (result == null)
            {
                return ResponseHelper.NotFound();
            }
            return ResponseHelper.Ok(result);
        }

    }
}
