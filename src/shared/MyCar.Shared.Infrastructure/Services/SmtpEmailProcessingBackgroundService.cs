using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyCar.Shared.Abstractions.Services;

namespace MyCar.Shared.Infrastructure.Services;
internal class SmtpEmailProcessingBackgroundService(
	IEmailServiceFactory emailServiceFactory,
	ILogger<SmtpEmailProcessingBackgroundService> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		while(!cancellationToken.IsCancellationRequested) {
			await ProcessEmailAsync();
			await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
		}
	}

	private async Task ProcessEmailAsync()
	{
		await Task.Run(async () =>
		{
			if(!EmailsQueue.IsEmpty && EmailsQueue.TryPeek(out Email email)) {
				var emailService = emailServiceFactory.GetService();
				if(await emailService.SendEmail(email, EmailsQueue.ErrorCount)) {
					if(EmailsQueue.TryDequeue(out email)) {
						logger.LogInformation($"Email {email.Subject} removed from queue.");
						EmailsQueue.ErrorCount = 0;
					}
					else {
						logger.LogError($"Error deleting email {email.Subject} from queue.");
					}
				}
				else { EmailsQueue.ErrorCount++; }
			}
		});
	}
}
