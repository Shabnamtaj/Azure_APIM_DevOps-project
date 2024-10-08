using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(string subject, string body)
    {
        var mail = new MailMessage();
        mail.From = new MailAddress("noreply@example.com");
        mail.To.Add("user@example.com");
        mail.Subject = subject;
        mail.Body = body;

        using var smtp = new SmtpClient(_config["Mailtrap:Host"], int.Parse(_config["Mailtrap:Port"]))
        {
            Credentials = new System.Net.NetworkCredential(_config["Mailtrap:User"], _config["Mailtrap:Password"]),
            EnableSsl = true
        };

        smtp.Send(mail);
    }
}
