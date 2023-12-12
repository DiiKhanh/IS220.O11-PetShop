using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.CartService;
using PetShop.Services.UserService;
using System.Linq;

namespace PetShop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderService(PetShopDbContext context, IMapper mapper, IUserService userService, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _cartService = cartService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> cancelOrder(int oid)
        {
            var userid = await _userService.loginUser();
            var order =  _context.Order.Find(oid);
            if (order == null) { return ResponseHelper.NotFound(); }
            var orderdt = await _context.OrderDetail.Where(o=>o.OrderId == oid).ToListAsync();
            orderdt.ForEach(o =>
            {
                var dogItem = _context.DogItem.Find(o.DogItemId);
                var dogProductItem = _context.DogProductItem.Find(o.DogProductItemId);
                if (dogItem != null)
                {
                    dogItem.IsInStock = true;
                }
                else if (dogProductItem != null)
                {
                    dogProductItem.Quantity += o.Quantity;
                }
                o.IsDeleted = true;
            });
            order.IsDeleted = true;
            _context.Invoice.Remove(_context.Invoice.Find(oid));
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok("Đã hủy đơn hàng thành công.");

        }

        public async Task<IActionResult> createOrder(ShipInfo request)
        {
            var userid = await _userService.loginUser();
            var cart = await _context.Cart.Include(c => c.cartDetails).ThenInclude(cd => cd.dogItems)
                                                 .Include(c => c.cartDetails).ThenInclude(cd => cd.dogProductItems)
                                                 .FirstOrDefaultAsync(c => c.CartId == userid);

            if (cart == null || cart.Total== 0)
            {
                return ResponseHelper.NotFound();
            }
            var shipinfo = _mapper.Map<ShipInfo>(request);
            var shipid = 0; 
            var findshipping = _context.ShipInfo.Where(y => y.UserId == userid).Where(x => x.Address.Trim().ToLower() == shipinfo.Address.Trim().ToLower())
                .FirstOrDefault();
            if ( findshipping == null) {
                shipinfo.UserId = userid;
                _context.ShipInfo.Add(shipinfo);
                _context.SaveChanges();
                shipid = (int)shipinfo.ShipInfoId;
            }
            else { shipid = (int)findshipping.ShipInfoId; }
            // Tạo order mới
            var order = new Order
            {
                UserId = userid,
                ShipInfoId = shipid,
                Total = (int) cart.Total,
                OrderStatus = Status.ChuaThanhToan,
                ShipmentStatus = Shipment.ChoXacNhan,
                CreateAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Tạo invoice
            var invoice = new Invoice
            {
                PaymentMethod = null
            };

            // Thêm order và invoice vào context
            _context.Order.Add(order);
            _context.Invoice.Add(invoice);
            _context.SaveChanges();

            // Thêm liên kết order-invoice
            order.Invoice = invoice;

            // Thêm các order detail từ cart
            foreach (var cartDetail in cart.cartDetails)
            {
                var dogItem = await _context.DogItem.FindAsync(cartDetail.DogItemId);
                var dogProductItem = await _context.DogProductItem.FindAsync(cartDetail.ProductItemId);
                var orderdt = new OrderDetail
                {
                    OrderId = order.OrderId
                };
                if (dogItem != null)
                {
                    dogItem.IsInStock = false;
                    orderdt.Quantity = 1;
                    orderdt.DogItemId = dogItem.DogItemId;
                    orderdt.CreateAt = DateTime.UtcNow;
                    orderdt.UpdatedAt = DateTime.UtcNow;
                    _context.OrderDetail.Add(orderdt);
                    await _context.SaveChangesAsync();
                }
                else if (dogProductItem != null)
                {
                    if (dogProductItem.Quantity < cartDetail.Quantity)
                    {
                        return ResponseHelper.BadRequest($"Số lượng sản phẩm {dogProductItem.ItemName} không đủ");
                    }
                    dogProductItem.Quantity -= cartDetail.Quantity;
                    orderdt.Quantity = cartDetail.Quantity;
                    orderdt.DogProductItemId = dogProductItem.DogProductItemId;
                    orderdt.CreateAt = DateTime.UtcNow;
                    orderdt.UpdatedAt = DateTime.UtcNow;
                    _context.OrderDetail.Add(orderdt);
                    await _context.SaveChangesAsync();
                }
            }
                // Xóa cart sau khi tạo order
                await _cartService.clearCart();

            // Lưu thay đổi
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok($"Đã tạo đơn hàng thành công. ID của đơn hàng là {order.OrderId}");
        }

        public async Task<IActionResult> getAllOrderAdmin()
        {
            var listorder = await _context.Order.Include(x => x.orderDetails).Include(y => y.ShipInfo)
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.Total,
                    ShipmentStatus = o.ShipmentStatus.ToString(),
                    OrderStatus = o.OrderStatus.ToString(),
                    o.orderDetails,
                    o.ShipInfo,
                    o.IsDeleted
                }).ToListAsync();
            return ResponseHelper.Ok(listorder);
        }
        public async Task<IActionResult> getAllOrder()
        {
            var userid = await _userService.loginUser();
            var listorder = await _context.Order.Include(x => x.orderDetails).Include(y=>y.ShipInfo).Where(d => d.IsDeleted != true 
            && d.UserId == userid)
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.Total,
                    ShipmentStatus = o.ShipmentStatus.ToString(),
                    OrderStatus = o.OrderStatus.ToString(),
                    o.orderDetails,
                    o.ShipInfo.Address,
                    o.ShipInfo.City,
                    o.ShipInfo.District
                }).ToListAsync();
            return ResponseHelper.Ok(listorder);
        }

        public async Task<IActionResult> getOrder(int oid)
        {
            var userid = await _userService.loginUser();
            var order = await _context.Order.Include(o => o.ShipInfo).Include(o => o.orderDetails).Select(o => new
            {
                o.OrderId,
                o.UserId,
                o.Total,
                ShipmentStatus = o.ShipmentStatus.ToString(),
                OrderStatus = o.OrderStatus.ToString(),
                o.orderDetails,
                o.ShipInfo.Address,
                o.ShipInfo.City,
                o.ShipInfo.District,
                o.IsDeleted
            }).FirstOrDefaultAsync(o => o.OrderId == oid);
            if (order == null || order.IsDeleted == true || order.UserId != userid) return ResponseHelper.NotFound();
            return ResponseHelper.Ok(order);
        }

        public async Task<IActionResult> getOrderAdmin(int oid)
        {
            var order = await _context.Order.Include(o=>o.ShipInfo).Include(o=>o.orderDetails).Select(o => new
            {
                o.OrderId,
                o.UserId,
                o.Total,
                ShipmentStatus = o.ShipmentStatus.ToString(),
                OrderStatus = o.OrderStatus.ToString(),
                o.orderDetails,
                o.ShipInfo.Address,
                o.ShipInfo.City,
                o.ShipInfo.District,
                o.IsDeleted
            }).FirstOrDefaultAsync(o => o.OrderId == oid);
            if (order == null) return ResponseHelper.NotFound();
            return ResponseHelper.Ok(order);
        }

        public async Task<IActionResult> updateOrder(int oid, OrderDto request)
        {
            var order = await _context.Order.Include(o => o.Invoice).Include(o=>o.orderDetails).FirstOrDefaultAsync(o => o.OrderId == oid);

            if (order == null)
            {
                return ResponseHelper.NotFound();
            }

            if (request.OrderStatus != null)
            {
                Status? orderStatus = Enum.Parse<Status>(request.OrderStatus);
                order.OrderStatus = orderStatus;
            }
            if (request.ShipmentStatus != null)
            {
                Shipment? shipmentStatus = Enum.Parse<Shipment>(request.ShipmentStatus);
                order.ShipmentStatus = shipmentStatus;
            }

            if (request.PaymentMethod != null)
            {
                PaymentMethod? paymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod);
                order.Invoice.PaymentMethod = paymentMethod;

            }
            await _context.SaveChangesAsync();
            return ResponseHelper.Ok(order);
        }

        public async Task<IActionResult> updateShipInfo(UpdateShipInfoDto request, int sid)
        {
            var userid = await _userService.loginUser();
            var findshipinfo = await _context.ShipInfo.FirstOrDefaultAsync(s=>s.UserId==userid&&s.ShipInfoId==sid);
            if (findshipinfo == null) {  return ResponseHelper.NotFound(); }
            findshipinfo.Address = request.Address ?? findshipinfo.Address;
            findshipinfo.City = request.City ?? findshipinfo.City;
            findshipinfo.District = request.District ?? findshipinfo.District;
            _context.SaveChanges();
            return ResponseHelper.Ok(new
            {
                findshipinfo.City,
                findshipinfo.District,
                findshipinfo.Address
            });
        }
    }
}
