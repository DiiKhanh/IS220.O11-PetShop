using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;

namespace PetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
  
        private readonly IConfiguration _configuration;
        private readonly PetShopDbContext _context;

        public DashboardController(PetShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDashboard()
        {
            var totalPet = await _context.DogItem.CountAsync();
            var totalProduct = await _context.DogProductItem.CountAsync();
            var totalUser = await _context.Users.CountAsync();
            var totalInvoice = await _context.Checkout.CountAsync();
            var totalMoney = await _context.Checkout.SumAsync(c => c.Total);
            var totalVoucher = await _context.Voucher.CountAsync();
            var checkoutData = await _context.Checkout
               .Select(c => new
               {
                   c.CreateAt,
                   c.Total
               })
               .ToListAsync();
            return ResponseHelper.Ok(new
            {
                totalPet,
                totalProduct,
                totalUser,
                totalInvoice,
                totalMoney,
                totalVoucher,
                checkoutData
            });
        }

        [HttpPost]
        [Route("get-invoice")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInvoice(DashboardDto request)
        {
            if (string.IsNullOrEmpty(request.Date))
            {
                return BadRequest("Date is required.");
            }

            // Chuyển đổi chuỗi ngày từ client thành DateTime
            DateTime dateToCompare;
            try
            {
                dateToCompare = DateTime.ParseExact(request.Date, "MM dd yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid date format. Please use 'MM dd yyyy'.");
            }

            // Lấy danh sách các hóa đơn từ cơ sở dữ liệu
            var checkoutData = await _context.Checkout.ToListAsync();

            // Lọc các hóa đơn theo ngày tháng năm
            var filteredData = checkoutData
                .Where(c => c.CreateAt?.Date == dateToCompare.Date)
                .ToList();

            // Tính toán thống kê
            var invoiceCount = filteredData.Count;
            var totalAmount = filteredData.Sum(c => c.Total); // Giả sử có thuộc tính Amount trong Checkout

            // Tạo đối tượng kết quả
            var result = new
            {
                Date = dateToCompare.ToString("MM dd yyyy"),
                InvoiceCount = invoiceCount,
                TotalAmount = totalAmount
            };

            return Ok(result);
        }

    }
}
