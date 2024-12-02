using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCar.Shared.Abstractions.Services;
using System.Text.Json;

namespace MyCar.Shared.Infrastructure.Services;

internal class FakeEmailService : IEmailService
{
	private readonly IOptionsMonitor<SmtpOptions> _smtpOptionsMonitor;
	private readonly ILogger<FakeEmailService> _logger;

	public FakeEmailService(IOptionsMonitor<SmtpOptions> smtpOptionsMonitor, ILogger<FakeEmailService> logger)
	{
		_smtpOptionsMonitor = smtpOptionsMonitor;
		_logger = logger;
	}

	private void OnChangeSmtpOptions(SmtpOptions options, string arg)
	{
		_logger.LogTrace(JsonSerializer.Serialize(options));
	}

	public async Task<bool> SendEmail(Email email, int counter)
	{
		_logger.LogWarning($"Smtp: {JsonSerializer.Serialize(_smtpOptionsMonitor.CurrentValue)}");

		await Task.CompletedTask;
		
		return false;
	}
}
