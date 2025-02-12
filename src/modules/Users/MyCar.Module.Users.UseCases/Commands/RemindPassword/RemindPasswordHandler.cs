﻿using MediatR;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Services;

namespace MyCar.Module.Users.UseCases.Commands.RemindPassword;
internal class RemindPasswordHandler(
	IUserRepository repository,
	IEmailConfirmerFactory emailConfirmerFactory) : IRequestHandler<RemindPasswordCommand>
{
	public async Task Handle(RemindPasswordCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetByEmailAsync(request.Email, cancellationToken)
			?? throw new InvalidCredentialsException();

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();

		var forgotEmail = new Email
		{
			Body = emailConfirmer.GetRemindPasswordBody(user.Id, user.Email),
			Subject = "Remind your password in the MyCar application",
			Recievers = [user.Email]
		};

		EmailsQueue.Add(forgotEmail);
	}
}
