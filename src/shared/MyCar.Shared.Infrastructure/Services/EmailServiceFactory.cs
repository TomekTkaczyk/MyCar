using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCar.Shared.Abstractions.Services;

namespace MyCar.Shared.Infrastructure.Services;
internal class EmailServiceFactory(
	IOptionsMonitor<SmtpOptions> options,
	ILogger<FakeEmailService> logger) : IEmailServiceFactory
{
	public IEmailService GetService()
	{
		return new FakeEmailService(options,logger);
	}
}
