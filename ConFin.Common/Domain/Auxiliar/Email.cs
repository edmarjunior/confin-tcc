using System.Net;
using System.Net.Mail;

namespace ConFin.Common.Domain.Auxiliar
{
    public static class Email
    {
        public static void Enviar(string subject, string emailTo, string body, string nomeTo = null)
        {
            var client = new SmtpClient
            {
                //UseDefaultCredentials = false,
                Host = "smtp-mail.outlook.com",
                EnableSsl = true,
                Credentials = new NetworkCredential("confinpessoal@outlook.com", "teste321"),
                Port = 587
            };

            var mail = new MailMessage
            {
                Sender = new MailAddress(emailTo, nomeTo),
                From = new MailAddress("confinpessoal@outlook.com", "ConFin automático"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(emailTo, nomeTo));
            client.Send(mail);
        }
    }
}
