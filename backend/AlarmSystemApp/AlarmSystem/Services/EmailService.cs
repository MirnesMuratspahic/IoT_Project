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

            if(message == "Fire")
            {
                message = "Poštovani, \n" + "\n" +
                          "Temperatura u prostoru je trenutno porasla. Na osnovu trenutnih podataka, postoji mogućnost da se " +
                          "razvije požar, te vas pozivamo da odmah provjerite stanje i preduzmete potrebne mjere opreza." + "\n" + "\n" +
                          "S poštovanjem, \n" +
                          "AlarmSystem";
            }
            else if(message == "Motion")
            {
                message = "Poštovani, \n" + "\n" +
                          "Detektovane su kretnje u prostoru. Postoji mogućnost provale unutar objekta,  " +
                          "te vas pozivamo da odmah provjerite stanje i preduzmete potrebne mjere opreza." + "\n" + "\n" +
                          "S poštovanjem, \n" +
                          "AlarmSystem";
            }


            // Podešavanje osnovnih parametara e-maila
            mail.From = new MailAddress(configuration["EmailAccountInformations:Email"]);
            mail.To.Add(userEmail);
            mail.Subject = "Test Email Subject";
            mail.Body = message; 



            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); 
            smtpServer.Port = 587; 
            smtpServer.Credentials = new NetworkCredential(configuration["EmailAccountInformations:Email"], configuration["EmailAccountInformations:Password"]);  // Tvoji podaci za prijavu
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
