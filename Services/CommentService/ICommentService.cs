using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;

namespace PetShop.Services.CommentService
{
    public interface ICommentService
    {
        Task<IActionResult> Create(CommentDto request);
        Task<IActionResult> GetCommentProduct(int product_id, Comment2Dto request);
        Task<IActionResult> GetAll();
        Task<IActionResult> Update(int id, Comment1Dto request);
    }
}
