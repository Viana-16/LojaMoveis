// Services/EmailService.cs
using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> EnviarEmailAsync(string para, string assunto, string corpoHtml)
    {
        try
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Email"],
                    _config["EmailSettings:Senha"]
                ),
                EnableSsl = true
            };

            var mail = new MailMessage(_config["EmailSettings:Email"], para, assunto, corpoHtml)
            {
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
