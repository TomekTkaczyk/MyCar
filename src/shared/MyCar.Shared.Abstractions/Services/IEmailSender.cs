﻿namespace MyCar.Shared.Abstractions.Services;
public interface IEmailSender
{
	Task<bool> SendEmailAsync(Email email, CancellationToken cancellationToken);
}
