using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.DogItemService;
using PetShop.Services.DogProductItemService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogProductItemController : ControllerBase
    {
        private readonly IDogProductItemService _dogProductItemService;

        public DogProductItemController(IDogProductItemService dogProductItemservice)
        {
            _dogProductItemService = dogProductItemservice;

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
        public async Task<IActionResult> Update(int id, DogProductItemDto product)
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

        [HttpGet("get-all-dog-product-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            try
            {
                var results = await _dogProductItemService.GetAllAdmin();
                return ResponseHelper.Ok(results);
            }
            catch (Exception ex)
            {
                return ResponseHelper.BadRequest(ex.Message);
            }

        }
    }
}
