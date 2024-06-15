namespace Cower.Service.Services;

public interface IEmailService
{
    public Task SendEmail(string receiver, string subject, string message);
}