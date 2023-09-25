using PetShop.Models;

namespace PetShop.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel request);
    }
}
