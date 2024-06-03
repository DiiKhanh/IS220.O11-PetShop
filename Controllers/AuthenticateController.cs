using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.UserService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        // phan nay se dung repositories(or services) to handle, tach biet controller va xu ly
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return await _userService.LoginAsync(model);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return await _userService.RegisterAsync(model);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            return await _userService.RegisterAdminAsync(model);
        }

        [HttpPost("verify/{token}")]
        public async Task<IActionResult> Verify(string token)
        {
            return await _userService.VerifyAsync(token);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return await _userService.ForgotPasswordAsync(email);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            return await _userService.ResetPasswordAsync(model);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // Sử dụng JwtSecurityTokenHandler để đọc token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenObj = tokenHandler.ReadJwtToken(token);

                // Lấy các claim từ token
                var claims = tokenObj.Claims;

                // Ví dụ: Lấy giá trị claim "sub" (Subject)
                var subClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
                if (subClaim != null)
                {
                    string userEmail = subClaim.Value;
                    // Sử dụng giá trị subject ở đây
                    return await _userService.ChangePasswordAsync(model, userEmail);
                }
            }
             return ResponseHelper.BadRequest("user email null");
        }
        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // Sử dụng JwtSecurityTokenHandler để đọc token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenObj = tokenHandler.ReadJwtToken(token);

                // Lấy các claim từ token
                var claims = tokenObj.Claims;

                // Ví dụ: Lấy giá trị claim "sub" (Subject)
                var subClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
                if (subClaim != null)
                {
                    string userEmail = subClaim.Value;
                    // Sử dụng giá trị subject ở đây
                    return await _userService.GetInfo(userEmail);
                }
            }
            return ResponseHelper.Error();
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return await _userService.GetAll();
        }

        [Authorize]
        [HttpPost("edit-info")]
        public async Task<IActionResult> EditInfo(UserDto request)
        {
            return await _userService.EditInfo(request);
        }

    }
}
