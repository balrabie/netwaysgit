using System.Net;
using System.Net.Mail;

namespace Services
{
    public class EmailManager
    {
        private MailAddress FromAddress = null;
        private string FromPassword = null;
        private MailAddress ToAddress = null;
        private SmtpClient SMTP = null;

        public string From
        {
            get => FromAddress.Address;
            set => FromAddress = new MailAddress(value);
        }

        public string To
        {
            get => ToAddress.Address;
            set => ToAddress = new MailAddress(value);
        }
        
        public string SetPassword
        {
            set => FromPassword = value;
        }

        public EmailManager()
        {
            SMTP = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(FromAddress.Address, FromPassword)
            };
        }

        public EmailManager(string host, int port)
        {
            SMTP = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(FromAddress.Address, FromPassword)
            };
        }


        public void SendMessage(string subject, string body)
        {
            if (ToAddress == null)
            {
                throw new System.Exception("Error: toAddress must be set before sending a message");
            }
            using (var message =
                new MailMessage(FromAddress, ToAddress)
                {
                    Subject = subject,
                    Body = body,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                }
                )
            {
                SMTP.Send(message);
            }

        }

    }
}
