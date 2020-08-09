using CleanBlog.Core.Controllers.Surface;
using CleanBlog.Core.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Logging;


namespace CleanBlog.Core.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly ILogger _logger;

        public SmtpService(ILogger logger)
        {
            _logger = logger;
        }

        public bool SendEmail(ContactViewModel model)
        {
            try
            {

                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                var toAddress = "messages@jamiesellars.io";
                var fromAddress = "no-reply@jamiesellars.io";

                message.Subject = $"Enquiry from {model.Name} - {model.Email}";
                message.Body = model.Message;
                message.To.Add(toAddress);
                message.From = new MailAddress(fromAddress);

                smtpClient.Send(message);
                return true;

            }
            catch (SmtpException exception)
            {
                _logger.Error(typeof(ContactSurfaceController), exception, "Error sending contact form");
                return false;
            }
        }
    }
}
