using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PetShop.Data;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.EmailService;

namespace PetShop.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly PetShopDbContext _context;
        private readonly IEmailService _emailService;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, PetShopDbContext context, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel model, string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return ResponseHelper.BadRequest("unauthorized");
            }
            var resetPasswordResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!resetPasswordResult.Succeeded)
            {
                return ResponseHelper.BadRequest("change pw fail. pls check password");
            }
            return ResponseHelper.Ok(new
            {
                message = "Change password success"
            });
        }

        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user is null)
                {
                    return ResponseHelper.BadRequest("User not exisits!");
                }

                user.PasswordResetToken = CreateRandomToken();
                user.ResetTokenExpires = DateTime.Now.AddHours(1);
                await _context.SaveChangesAsync();

                var mail = new EmailModel
                {
                    To = email,
                    Subject = "Please verify your account forgot - PetShop",
                    Body = "<p>Hey! You've just tried to reset your password for your account - " +
                            "Please reset it using this token: " +
                            $"{user.PasswordResetToken}</p>"
                };

                _emailService.SendEmail(mail);

                return ResponseHelper.Ok(new
                {
                    email,
                    message = "You may now reset your password."
            });
            }
            catch (Exception e)
            {
                return ResponseHelper.BadRequest($"{e.Message}");
            }
        }

        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.VerifiedAt is null)
                {
                    return ResponseHelper.BadRequest("Email not verified!");
                }

                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = GetToken(authClaims);

                var data = new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo, id = user.Id, email = user.Email, username = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    phoneNumber = user.PhoneNumber,
                };
                return ResponseHelper.Ok(data);
            }
            return ResponseHelper.Unauthorized();
        }

        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email) ?? await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return ResponseHelper.BadRequest("User already exists!");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow,
                VerificationToken = CreateRandomToken()
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return ResponseHelper.BadRequest("User creation failed! Please check user details and try again.");
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }

            EmailModel verificationEmail = CreateVerificationEmail(model, user);
            _emailService.SendEmail(verificationEmail);
            return ResponseHelper.Created(new
            {
                message = "User created successfully and Please check your email to verify your user!",
                email = user.Email
            });
        }

        public async Task<IActionResult> RegisterAdminAsync(RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email) ?? await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return ResponseHelper.BadRequest("User already exists!");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow,
                VerificationToken = CreateRandomToken()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return ResponseHelper.BadRequest("User creation failed! Please check user details and try again.");

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
           

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            EmailModel verificationEmail = CreateVerificationEmail(model, user);
            _emailService.SendEmail(verificationEmail);

            return ResponseHelper.Created(new
            {
                message = "User created successfully and Please check your email to verify your user!",
                email = user.Email
            });
        }

        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == model.Token);
            if (user is null || user.ResetTokenExpires < DateTime.Now)
            {
                return ResponseHelper.BadRequest("Invalid Token");
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!resetPasswordResult.Succeeded)
            {
                return ResponseHelper.BadRequest("reset password fail, pls check token, current and new password");
            }
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return ResponseHelper.Ok(new
            {
                email = user.Email,
                message = "Reset password success!"
            });
        }

        public async Task<IActionResult> VerifyAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user is null)
            {
                return ResponseHelper.BadRequest("Token not verified!");
            }

            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ResponseHelper.Ok(new
            {
                message = "Your account verified success!",
                email = user.Email
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private static string CreateRandomToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string token = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return token;
        }

        private static EmailModel CreateVerificationEmail(RegisterModel model, ApplicationUser user)
        {
            var mail = new EmailModel
            {
                To = model.Email!,
                Subject = "Please verify your email",
                Body = $@"
                        <h1>Email Confirmation</h1>
                        <p>Dear {user.UserName},</p>
                        <p>Cảm ơn bạn đã đăng ký tài khoản! Đây là mã xác nhận của bạn:</p>
                        <p><strong>{user.VerificationToken}</strong></p>
                        <p>Vui lòng sử dụng mã này để xác nhận địa chỉ email của bạn. Sau khi xác nhận, bạn sẽ có quyền truy cập vào tài khoản của mình.</p>
                        <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                        <p>Trân trọng,</p>
                        <p>PetShop@2023</p>"
        };

            return mail;
        }

        public async Task<IActionResult> GetInfo(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return ResponseHelper.Unauthorized();
            }
            return ResponseHelper.Ok(new
            {
                username = user.UserName,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                phoneNumber = user.PhoneNumber,
            });
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            if (users is null) return ResponseHelper.NotFound();
            List<object> responselist = new List<object>();
            users.ForEach(user =>
            {
                

                object response = new
                {
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.CreatedAt,
                    user.Id
                };
                responselist.Add(response);
            });
            return ResponseHelper.Ok(responselist);
        }
    }
}
