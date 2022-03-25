namespace MakeYourBusinessGreen.Application.Interfaces;
public interface IMailService
{
    Task SendAsync(string to, string subject, string htmlBody);
}
