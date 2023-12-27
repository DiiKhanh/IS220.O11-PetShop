using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Services.CommentService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody] CommentDto request)
        {
            
            return await _commentService.Create(request);
        }
        [HttpPost("product-comment/{id}")]
        public async Task<IActionResult> GetCommentProduct([FromRoute] int id, Comment2Dto request)
        {

            return await _commentService.GetCommentProduct(id, request);
        }
        [HttpGet("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {

            return await _commentService.GetAll();
        }
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, Comment1Dto request)
        {

            return await _commentService.Update(id, request);
        }
    }
}
