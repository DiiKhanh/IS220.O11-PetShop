using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Models;

namespace PetShop.Services.UserService
{
    public interface  IUserService
    {
        Task<IActionResult> RegisterAsync(RegisterModel model);
        Task<IActionResult> RegisterAdminAsync(RegisterModel model);
        Task<IActionResult> LoginAsync(LoginModel model);
        Task<IActionResult> VerifyAsync(string token);
        Task<IActionResult> ForgotPasswordAsync(string email);
        Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model);
        Task<IActionResult> ChangePasswordAsync(ChangePasswordModel model, string userEmail);
        Task<IActionResult> GetInfo(string userEmail);
        Task<IActionResult> GetAll();

        Task<IActionResult> EditInfo(UserDto request);
    }
}
