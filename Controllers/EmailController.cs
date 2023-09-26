using Microsoft.AspNetCore.Mvc;
using PetShop.Helpers;
using PetShop.Models;
using PetShop.Services.EmailService;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailModel request)
        {
            _emailService.SendEmail(request);
            return ResponseHelper.Ok(new
            {
                message = "Email was send",
                receiver = request.To
            });
        }
    }
}
