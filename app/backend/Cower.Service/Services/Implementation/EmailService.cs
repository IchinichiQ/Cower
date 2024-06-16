using MailKit.Net.Smtp;
using MimeKit;

namespace Cower.Service.Services.Implementation;

public class EmailService : IEmailService
{
    private readonly string SMTP_SERVER_ADDRESS;
    private readonly int SMTP_SERVER_PORT;
    private readonly string SENDER_EMAIL;
    private readonly string SMTP_USERNAME;
    private readonly string SMTP_PASSWORD;

    public EmailService()
    {
        SMTP_SERVER_ADDRESS = Environment.GetEnvironmentVariable("SMTP_SERVER_ADDRESS")!;
        SMTP_SERVER_PORT = int.Parse(Environment.GetEnvironmentVariable("SMTP_SERVER_PORT")!);
        SENDER_EMAIL = Environment.GetEnvironmentVariable("SENDER_EMAIL")!;
        SMTP_USERNAME = Environment.GetEnvironmentVariable("SMTP_USERNAME")!;
        SMTP_PASSWORD = Environment.GetEnvironmentVariable("SMTP_PASSWORD")!;
    }

    public async Task SendEmail(
        string receiver,
        string subject,
        string message)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Коворкинг Cowёr", SENDER_EMAIL));
        email.To.Add(new MailboxAddress("Пользователь сервиса", receiver));

        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { 
            Text = message
        };
        
        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(SMTP_SERVER_ADDRESS, SMTP_SERVER_PORT, true);
            
            await smtp.AuthenticateAsync(SMTP_USERNAME, SMTP_PASSWORD);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}