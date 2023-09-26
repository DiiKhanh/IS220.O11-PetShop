using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using PetShop.Models;

namespace PetShop.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_config["EmailSettings:EmailHost"], 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config["EmailSettings:EmailUsername"], _config["EmailSettings:EmailPassword"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            
        }
    }
}
