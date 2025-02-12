﻿using MediatR;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Services;

namespace MyCar.Module.Users.UseCases.Commands.ChangeEmail;

internal class ChangeEmailHandler(
	IUserRepository repository,
	ITokenProvider tokenProvider) : IRequestHandler<ChangeEmailCommand>
{
	public async Task Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(request.Id, cancellationToken)
		?? throw new InvalidCredentialsException();

		var email = request.Email.ToLowerInvariant();

		var anotherUser = await repository.GetByEmailAsync(email, cancellationToken);

		if(anotherUser is not null) {
			if(!user.Id.Equals(anotherUser.Id)) {
				throw new EmailIsInUseException();
			}
			if(user.Email.Equals(email) && user.EmailConfirm) {
				throw new EmailNotChanged();
			}
		}

		var token = tokenProvider.GenerateConfirmEmailToken(request.Id, email);

		user.EmailConfirmToken = token;
		user.EmailToConfirm = email;

		await CreateEmail(
			request.ConfirmEmailUrl + "?token=" + token,
			email,
			cancellationToken);

		await repository.UpdateAsync(user, cancellationToken);
	}

	private static async Task CreateEmail(string confirmEmailUrl, string emailAddress, CancellationToken cancellationToken)
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "EmailConfirmTokenTemplate.html");
		var template = await File.ReadAllTextAsync(path, cancellationToken);
		var email = new Email
		{
			Body = template.Replace("{{ConfirmUrl}}", confirmEmailUrl),
			Subject = "Potwierdzenie adresu email w aplikacji MyCar",
			Recievers = [emailAddress]
		};
		EmailsQueue.Add(email);
	}
}
