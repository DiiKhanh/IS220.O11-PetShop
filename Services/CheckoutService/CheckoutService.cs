using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.EmailService;

namespace PetShop.Services.CheckoutService
{
    public class CheckoutService : ICheckoutService
    {
        private readonly PetShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CheckoutService(PetShopDbContext context, IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        
        public async Task<IActionResult> Create(CheckoutDto request)
        {

            string dataJson = JsonConvert.SerializeObject(request.Data);
            dynamic data = JsonConvert.DeserializeObject<DataObject[]>(dataJson);

            foreach (DataObject item in data)
            {
                if (item.type == "animal")
                {
                    DogItem dogItem = _context.DogItem.Find(item.id);

                    if (dogItem == null)
                    {
                        return ResponseHelper.BadRequest("Không tìm thấy sản phẩm");
                    }

                    if (dogItem.IsInStock == false)
                    {
                        return ResponseHelper.BadRequest("Sản phẩm đã hết vui lòng kiểm tra lại!");
                    }

                    dogItem.IsInStock = false;
                    _context.SaveChanges();
                }
                else if (item.type == "product")
                {
                    DogProductItem productItem = _context.DogProductItem.Find(item.id);

                    if (productItem == null)
                    {
                        return ResponseHelper.BadRequest("Không tìm thấy sản phẩm");
                    }

                    if (productItem.IsInStock == false)
                    {
                        return ResponseHelper.BadRequest("Sản phẩm đã hết vui lòng kiểm tra lại!");
                    }

                    var temp = productItem.Quantity - item.quantity;

                    if (temp <= 0)
                    {
                        productItem.IsInStock = false;
                        if (productItem.IsInStock == false)
                        {
                            return ResponseHelper.BadRequest("Sản phẩm đã hết vui lòng kiểm tra lại!");
                        }
                    }
                    productItem.Quantity = item.stock - item.quantity;

                    _context.SaveChanges();
                }
            }


            var checkout = _mapper.Map<Checkout>(request);
            checkout.Data = JsonConvert.SerializeObject(request.Data);
            checkout.CreateAt = DateTime.UtcNow;
            checkout.UpdatedAt = DateTime.UtcNow;
            await _context.Checkout.AddAsync(checkout);
            await _context.SaveChangesAsync();
            // dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);
            return ResponseHelper.Ok(new
            {
               status = 201
            });
        }

        public async Task<IActionResult> GetByUser(string user_id)
        {
            var checkouts = await _context.Checkout.Where(d => d.User_id == user_id).ToListAsync();
            if (checkouts is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            checkouts.ForEach(checkout =>
            {
                dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);
                
                object response = new
                {
                    checkout.Id,
                    checkout.User_id,
                    Data,
                    checkout.Address,
                    checkout.Status,
                    checkout.Payment,
                    checkout.CreateAt,
                    checkout.UpdatedAt,
                    checkout.Email,
                    checkout.Name,
                    checkout.PhoneNumber,
                    checkout.Total
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);

            //var checkout = await _context.Checkout.FirstOrDefaultAsync(x => x.User_id == user_id);
            //if (checkout is null) return ResponseHelper.NotFound();
            //dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);
            
        }

        public async Task<IActionResult> SendEmailCheckout(EmailCheckoutDto request)
        {
            var mail = new EmailModel
            {
                To = request.Email,
                Subject = "Thông tin đơn hàng - PetShop",
                Body = $@"
        <html>
            <body>
                <h2>Xin chào {request.Name},</h2>
                <p>Cảm ơn bạn đã đặt hàng tại PetShop. Dưới đây là chi tiết của đơn hàng:</p>
                
                <h3>Thông tin đơn hàng</h3>
                <ul>
                    <li><strong>Địa chỉ giao hàng:</strong> {request.Address}</li>
                    <li><strong>Số điện thoại người nhận:</strong> {request.Phone}</li>
                </ul>
                
                <h3>Kiểm tra đơn hàng trong lịch sử mua hàng trên websitet</h3>
          
                
                <h3>Tổng cộng thanh toán: {request.Total}vnd</h3>
                
                <p>Cảm ơn bạn đã mua sắm tại PetShop. Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                
                <p>Trân trọng,</p>
                <p>Đội ngũ PetShop</p>
            </body>
        </html>"
            };

            _emailService.SendEmail(mail);
            return ResponseHelper.Ok(new
            {
                status = 200,
                data = "Đã gửi email hóa đơn, vui lòng kiểm tra!"
            });
        }

        public async Task<IActionResult> GetDetail(int id)
        {
            var checkout = await _context.Checkout.FirstOrDefaultAsync(x => x.Id == id);
            if (checkout is null) return ResponseHelper.NotFound();
            dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);
            return ResponseHelper.Ok(new
            {
                checkout.Id,
                checkout.User_id,
                Data,
                checkout.Address,
                checkout.Status,
                checkout.Payment,
                checkout.CreateAt,
                checkout.UpdatedAt,
                checkout.Email,
                checkout.Name,
                checkout.PhoneNumber,
                checkout.Total
            });
        }

        public async Task<IActionResult> CheckoutVnpay(CheckoutVnpayDto request, string ip)
        {
            string dataJson = JsonConvert.SerializeObject(request.Data);
            dynamic data = JsonConvert.DeserializeObject<DataObject[]>(dataJson);
            foreach (DataObject item in data)
            {
                if (item.type == "animal")
                {
                    DogItem dogItem = _context.DogItem.Find(item.id);

                    if (dogItem == null)
                    {
                        return ResponseHelper.BadRequest("Không tìm thấy sản phẩm");
                    }

                    if (dogItem.IsInStock == false)
                    {
                        return ResponseHelper.BadRequest("Sản phẩm đã hết vui lòng kiểm tra lại!");
                    }
                }
                else if (item.type == "product")
                {
                    DogProductItem productItem = _context.DogProductItem.Find(item.id);

                    if (productItem == null)
                    {
                        return ResponseHelper.BadRequest("Không tìm thấy sản phẩm");
                    }

                    if (productItem.IsInStock == false)
                    {
                        return ResponseHelper.BadRequest("Sản phẩm đã hết vui lòng kiểm tra lại!");
                    }

                }
            }

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            Random random = new Random();
            int vnp_TxnRef = random.Next(1, 10001);
            DateTime startTime = DateTime.Now; 

            DateTime expire = startTime.AddMinutes(15);

            string expireString = expire.ToString("yyyyMMddHHmmss");

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)request.Total * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", ip);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toan GD:{vnp_TxnRef}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", expireString);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return ResponseHelper.Ok(new
            {
                data = paymentUrl
            });
        }

        public async Task<IActionResult> GetAll()
        {
            var checkouts = await _context.Checkout.ToListAsync();
            if (checkouts is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            checkouts.ForEach(checkout =>
            {
                dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);

                object response = new
                {
                    checkout.Id,
                    checkout.User_id,
                    Data,
                    checkout.Address,
                    checkout.Status,
                    checkout.Payment,
                    checkout.CreateAt,
                    checkout.UpdatedAt,
                    checkout.Email,
                    checkout.Name,
                    checkout.PhoneNumber,
                    checkout.Total
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }

        public async Task<IActionResult> Update(int id, InvoiceDto request)
        {

            var checkout =
                await _context.Checkout.FirstOrDefaultAsync(x => x.Id == id);
            if (checkout is null) return ResponseHelper.NotFound();
            
            try
            {
                checkout.Name = request.Name;
                checkout.Address = request.Address;
                checkout.Status = request.Status;
                checkout.PhoneNumber = request.PhoneNumber;
                checkout.Payment = request.Payment;
                checkout.Email = request.Email;
                checkout.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ResponseHelper.BadRequest("Không thể cập nhật. Vui lòng thử lại");
            }

            dynamic Data = JsonConvert.DeserializeObject<DataObject[]>(checkout.Data);
            return ResponseHelper.Ok(new
            {
                checkout.Id,
                checkout.User_id,
                Data,
                checkout.Address,
                checkout.Status,
                checkout.Payment,
                checkout.CreateAt,
                checkout.UpdatedAt,
                checkout.Email,
                checkout.Name,
                checkout.PhoneNumber,
                checkout.Total
            });
        }

    }
}