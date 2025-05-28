using AlarmSystem.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;



namespace AlarmSystem.Services
{
    public class EmailService : IEmailService
    {
        public IConfiguration configuration { get; set; }
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(string message, string userEmail)
        {
            MailMessage mail = new MailMessage();

            if (message == "Fire")
            {
                message = "Dear Sir/Madam,\n\n" +
                          "The temperature in the area has risen. Based on the current data, there is a possibility of a fire developing, " +
                          "so we urge you to immediately check the situation and take the necessary precautionary measures.\n\n" +
                          "Sincerely,\n" +
                          "AlarmSystem";
            }
            else if (message == "Motion")
            {
                message = "Dear Sir/Madam,\n\n" +
                          "Motion has been detected in the area. There is a possibility of a break-in within the premises, " +
                          "so we urge you to immediately check the situation and take the necessary precautionary measures.\n\n" +
                          "Sincerely,\n" +
                          "AlarmSystem";
            }


            mail.From = new MailAddress(configuration["EmailAccountInformations:Email"]);
            mail.To.Add(userEmail);
            mail.Subject = "Alarm system notification";
            mail.Body = message; 



            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); 
            smtpServer.Port = 587; 
            smtpServer.Credentials = new NetworkCredential(configuration["EmailAccountInformations:Email"], configuration["EmailAccountInformations:Password"]);
            smtpServer.EnableSsl = true;

            try
            {
                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
                
            }
        }

        public async Task SendEmailWithCode(string code, string email)
        {
            MailMessage mail = new MailMessage();

            string message = "Dear Sir/Madam,\n\n" +
                             $"Your email verification code is: {code}\n\n" +
                             "Verification link: http://localhost:4200/code\n" +
                             "Sincerely,\n" +
                             "AlarmSystem";

            mail.From = new MailAddress(configuration["EmailAccountInformations:Email"]);
            mail.To.Add(email);
            mail.Subject = "Email Address Verification";
            mail.Body = message;



            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(configuration["EmailAccountInformations:Email"], configuration["EmailAccountInformations:Password"]);
            smtpServer.EnableSsl = true;

            try
            {
                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {

            }


        }
    }

}
