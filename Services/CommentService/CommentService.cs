using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;
        public CommentService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Create(CommentDto request)
        {
            var comment = _mapper.Map<Comment>(request);
            comment.CreateAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;
            comment.IsAccept = false;
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                status = 201,
                data = comment
            });
        }
        public async Task<IActionResult> GetCommentProduct(int product_id, Comment2Dto request)
        {

            var comments = await _context.Comment.Where(x => x.Product_id == product_id && x.Type == request.Type && x.IsAccept == true).ToListAsync();
            if (comments is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            comments.ForEach(comment =>
            {
               
                    object response = new
                    {
                        comment.Comment_id,
                        comment.User_id,
                        comment.Content,
                        comment.Product_id,
                        comment.Type,
                        comment.CreateAt,
                        comment.UpdatedAt,
                        comment.Username,
                        comment.IsAccept
                    };
                    responselist.Add(response);
             

            });
            return ResponseHelper.Ok(responselist);
        }
        public async Task<IActionResult> GetAll()
        {
            var comments = await _context.Comment.ToListAsync();
            if (comments is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            comments.ForEach(comment =>
            {
          
                object response = new
                {
                    comment.Comment_id,
                    comment.User_id,
                    comment.Content,
                    comment.Product_id,
                    comment.Type,
                    comment.CreateAt,
                    comment.UpdatedAt,
                    comment.Username,
                    comment.IsAccept
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);

        }
        public async Task<IActionResult> Update(int id, Comment1Dto request)
        {
            var comment =
                 await _context.Comment.FirstOrDefaultAsync(x => x.Comment_id == id);
            if (comment is null) return ResponseHelper.NotFound();

            try
            {
                comment.IsAccept = request.IsAccept;
                comment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return ResponseHelper.Ok(new
                {
                    comment.Comment_id,
                    comment.User_id,
                    comment.Content,
                    comment.Product_id,
                    comment.Type,
                    comment.CreateAt,
                    comment.UpdatedAt,
                    comment.Username,
                    comment.IsAccept
                });
            }
            catch (Exception)
            {
                return ResponseHelper.BadRequest("Không thể cập nhật. Vui lòng thử lại");
            }

           
        }
    }
}
