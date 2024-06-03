using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;


        public VoucherService(PetShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Create(VoucherDto request)
        {
            var existVoucher =
                await _context.Voucher.FirstOrDefaultAsync(e => e.Code.ToLower().Trim() == request.Code.ToLower().Trim());
            if (existVoucher is not null) return ResponseHelper.BadRequest("Mã voucher trùng!");
            var voucher = _mapper.Map<Voucher>(request);
            voucher.CreateAt = DateTime.UtcNow;
            voucher.UpdatedAt = DateTime.UtcNow;
            voucher.IsDeleted = false;
            await _context.Voucher.AddAsync(voucher);
            await _context.SaveChangesAsync();

            return ResponseHelper.Ok(new
            {
                status = 201
            });
        }

        public async Task<IActionResult> GetAllAdmin()
        {
            var vouchers = await _context.Voucher.ToListAsync();
            List<object> responselist = new List<object>();
            vouchers.ForEach(voucher =>
            {
                object response = new
                {
                 voucher.IsDeleted,
                 voucher.Current_usage,
                 voucher.Max_usage,
                 voucher.Code,
                 voucher.Voucher_id,
                 voucher.Start_date, voucher.End_date,
                 voucher.Discount_type, voucher.Discount_value,
                 voucher.CreateAt,
                 voucher.UpdatedAt
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var voucher = _context.Voucher.FirstOrDefault(x => x.Voucher_id == id);
            if (voucher is null || voucher.IsDeleted == true) return ResponseHelper.NotFound();
            //_context.DogItem.Remove(dogitem);
            voucher.IsDeleted = true;
            voucher.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                status = 200,
                data = "Delete success"
            });
        }

        public async Task<IActionResult> GetCode(string code)
        {
            var voucher = _context.Voucher.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Code == code);
            if (voucher is null || voucher.IsDeleted == true) return ResponseHelper.NotFound();
         
            if (voucher.Current_usage >= voucher.Max_usage) return ResponseHelper.NotFound();
            voucher.Current_usage += 1;

            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(new
            {
                voucher.IsDeleted,
                voucher.Current_usage,
                voucher.Max_usage,
                voucher.Code,
                voucher.Voucher_id,
                voucher.Start_date,
                voucher.End_date,
                voucher.Discount_type,
                voucher.Discount_value,
                voucher.CreateAt,
                voucher.UpdatedAt
            });
        }

        public async Task<IActionResult> List()
        {
            var vouchers = await _context.Voucher.Where(x => x.IsDeleted == false).ToListAsync();
            List<object> responselist = new List<object>();
            vouchers.ForEach(voucher =>
            {
                object response = new
                {
                    voucher.IsDeleted,
                    voucher.Current_usage,
                    voucher.Max_usage,
                    voucher.Code,
                    voucher.Voucher_id,
                    voucher.Start_date,
                    voucher.End_date,
                    voucher.Discount_type,
                    voucher.Discount_value,
                    voucher.CreateAt,
                    voucher.UpdatedAt
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> GetCodeAdmin(int id)
        {
            var voucher = _context.Voucher.FirstOrDefault(x => x.Voucher_id == id);
            if (voucher is null) return ResponseHelper.NotFound();
            return ResponseHelper.Ok(new
            {
                voucher.IsDeleted,
                voucher.Current_usage,
                voucher.Max_usage,
                voucher.Code,
                voucher.Voucher_id,
                voucher.Start_date,
                voucher.End_date,
                voucher.Discount_type,
                voucher.Discount_value,
                voucher.CreateAt,
                voucher.UpdatedAt
            });
        }

        public async Task<IActionResult> Edit(int id, VoucherDto request)
        {

            var voucher = _context.Voucher.FirstOrDefault(x => x.Voucher_id == id);
            if (voucher is null) return ResponseHelper.NotFound();

            voucher.UpdatedAt = DateTime.UtcNow;
            voucher.Code = request.Code;
            voucher.Start_date = request.Start_date;
            voucher.End_date = request.End_date;
            voucher.Max_usage = request.Max_usage;
            voucher.Discount_value = request.Discount_value;
            voucher.Discount_type = request.Discount_type;
            voucher.IsDeleted = request.IsDeleted;
            await _context.SaveChangesAsync();

            return ResponseHelper.Ok(new
            {
                status = 200,
                voucher.IsDeleted,
                voucher.Current_usage,
                voucher.Max_usage,
                voucher.Code,
                voucher.Voucher_id,
                voucher.Start_date,
                voucher.End_date,
                voucher.Discount_type,
                voucher.Discount_value,
                voucher.CreateAt,
                voucher.UpdatedAt
            });
        }
    }
}
