using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;

namespace PetShop.Services.CheckoutService
{
    public interface ICheckoutService
    {
        Task<IActionResult> Create(CheckoutDto request);
        Task<IActionResult> GetByUser(string user_id);
        Task<IActionResult> SendEmailCheckout(EmailCheckoutDto request);
        Task<IActionResult> GetDetail(int id);
        Task<IActionResult> CheckoutVnpay(CheckoutVnpayDto request, string ip);
        Task<IActionResult> GetAll();
        Task<IActionResult> Update(int id, InvoiceDto request);
    }
}
