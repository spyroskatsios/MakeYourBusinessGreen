using Microsoft.Extensions.Logging;

namespace MakeYourBusinessGreen.Infrastructure.Services;
public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;

    public MailService(ILogger<MailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string htmlBody)
    {
        _logger.LogInformation("Email to: {0}, with subject: {1} and body: {2}", to, subject, htmlBody);
        return Task.CompletedTask;
    }
}
