using System.Net.Mail;
using System.Net;

namespace API.Helper.Services
{
    public class MyEmailSenderService
    {

        private readonly IConfiguration _configuration;

        public MyEmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to, string messageSubject, string messageBody, bool isBodyHtml = false)
        {
            if (to == "")
                return;

            string host = this._configuration.GetValue<string>("MyEmailServer:Host");
            int port = this._configuration.GetValue<int>("MyEmailServer:Port");
            string from = this._configuration.GetValue<string>("MyEmailServer:From");
            string password = this._configuration.GetValue<string>("MyEmailServer:Password");

            SmtpClient SmtpServer = new SmtpClient(host, port);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage email = new MailMessage();
            email.From = new MailAddress(from);
            email.To.Add(to);
            email.Subject = messageSubject;
            email.Body = messageBody;
            email.IsBodyHtml = isBodyHtml;
            SmtpServer.Timeout = 5000;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(from, password);
            SmtpServer.Send(email);
        }

    }
}
