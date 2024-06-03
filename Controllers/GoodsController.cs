using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoodsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;

        public GoodsController(PetShopDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var goods = await _context.Goods.ToListAsync();
            List<object> responselist = new List<object>();
            goods.ForEach(good =>
            {
                object response = new
                {
                    good.ProductName,
                    good.Quantity, good.Price,
                    good.Total, good.Note, good.Address,
                    good.PhoneNumber, good.CreateAt,
                    good.GoodsId,
                    good.Supplier
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        [HttpPost]
        [Route("create-goods")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateImportGoods(GoodsDto request)
        {
            var goods = _mapper.Map<Goods>(request);
            goods.Supplier = request.Supplier;
            goods.Price = request.Price;
            goods.Total = request.Quantity * request.Price;
            goods.Quantity = request.Quantity;
            goods.Address = request.Address;
            goods.PhoneNumber = request.PhoneNumber;
            goods.CreateAt = DateTime.UtcNow;
            goods.ProductName = request.ProductName;
            goods.Note = request.Note;
            await _context.Goods.AddAsync(goods);
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                goods.Supplier,
                goods.Price,
                goods.Total,
                goods.Quantity,
                goods.Address,
                goods.PhoneNumber,
                goods.CreateAt,
                goods.ProductName,
                goods.Note,
                goods.GoodsId
        });
        }

    }
}
