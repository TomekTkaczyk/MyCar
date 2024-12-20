using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MyCar.Shared.Abstractions.Services;
using System.Text.Json;

namespace MyCar.Shared.Infrastructure.Services;
internal class EmailService : IEmailService
{
	private readonly IOptionsMonitor<SmtpOptions> _optionsMonitor;
	private readonly ILogger<IEmailService> _logger;
	private SmtpOptions _options;

	public EmailService(IOptionsMonitor<SmtpOptions> optionsMonitor, ILogger<IEmailService> logger)
	{
		_logger = logger;
		_optionsMonitor = optionsMonitor;
		_optionsMonitor.OnChange(OnConfigurationChanged);
		if(!IsValidConfiguration(_optionsMonitor.CurrentValue, out var errors)) {
			_logger.LogError("Invalid SMTP configuration detected: {Errors}", string.Join(", ", errors));
		}
	}

	public async Task<bool> SendEmail(Email email, int counter)
	{
		var options = _optionsMonitor.CurrentValue;
		if(!IsValidConfiguration(options, out var errors)) {
			_logger.LogError($"[{counter}] Invalid SMTP configuration detected: {string.Join(", ", errors)}");
			return false;
		}

		try {
			using var client = new SmtpClient();
			await client.ConnectAsync(
				host: options.Host,
				port: options.Port,
				options: SecureSocketOptions.StartTls);
			await client.AuthenticateAsync(_options.Account, _options.Password);
			await client.SendAsync(CreateEmail(email));
			await client.DisconnectAsync(true);
			client.Dispose();
		}
		catch(Exception ex) {
		}

		return true;
	}

	private void OnConfigurationChanged(SmtpOptions newOptions)
	{
		_logger.LogInformation("OnConfigurationChanged invoked.");
		_options = newOptions;
		if(!IsValidConfiguration(newOptions, out var errors)) {
			_logger.LogError("Invalid SMTP configuration detected: {Errors}", string.Join(", ", errors));
		}
		else {
			_logger.LogInformation("SMTP configuration updated successfully and is valid.");
		}
	}

	private static bool IsValidConfiguration(SmtpOptions options, out List<string> errors)
	{
		errors = [];
		if(string.IsNullOrEmpty(options.Host)) {
			errors.Add("SMTP host required.");
		}
		if(options.Port <= 0) {
			errors.Add("SMTP port must by a positive number.");
		}
		if(string.IsNullOrWhiteSpace(options.Password)) {
			errors.Add("SMTP Password is required.");
		}
		if(string.IsNullOrWhiteSpace(options.Issuer)) {
			errors.Add("SMTP Issuer is required.");
		}
		if(string.IsNullOrWhiteSpace(options.IssuerEmail)) {
			errors.Add("SMTP IssuerEmail is required.");
		}

		return errors.Count == 0;
	}

	private MimeMessage CreateEmail(Email email)
	{
		var message = new MimeMessage()
		{
			Subject = email.Subject,
			Body = new TextPart(MimeKit.Text.TextFormat.Text)
			{
				Text = email.Body
			}
		};
		message.From.Add(new MailboxAddress(_options.Issuer, _options.IssuerEmail));
		foreach(var address in email.Recievers) {
			message.To.Add(new MailboxAddress("", address));
		}

		return message;
	}
}
