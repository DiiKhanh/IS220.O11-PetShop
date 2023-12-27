using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;

namespace PetShop.Services.VoucherService
{
    public interface IVoucherService
    {
        Task<IActionResult> Create(VoucherDto request);
        Task<IActionResult> GetAllAdmin();
        Task<IActionResult> Delete(int id);
        Task<IActionResult> GetCode(string code);
        Task<IActionResult> List();
    }
}
