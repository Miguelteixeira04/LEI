using System;
using System.Net;
using System.Net.Mail;

namespace Utad.Lab.PL4.G01
{
    public class EmailSender
    {
        private readonly string _fromEmail = "compras.mike2004@gmail.com";
        private readonly string _password = "unpqgyjznpntbrhs"; // Senha de aplicativo, sem espaços

        public void SendEmail(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentException("O endereço de email não pode ser vazio.", nameof(toEmail));
            }

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromEmail, _password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP specific exceptions
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle other possible exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
