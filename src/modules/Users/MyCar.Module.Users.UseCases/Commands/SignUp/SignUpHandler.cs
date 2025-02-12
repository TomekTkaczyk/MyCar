﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Exceptions;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Services;

namespace MyCar.Module.Users.UseCases.Commands.SignUp;
internal class SignUpHandler(
	IUserRepository repository,
	IPasswordHasher<User> passwordHasher,
	ITokenProvider tokenProvider,
	IClock clock) : IRequestHandler<SignUpCommand, Guid>
{
	public async Task<Guid> Handle(SignUpCommand request, CancellationToken cancellationToken)
	{
		var error = new ApiError();
		var user = await repository.GetByEmailAsync(request.Email.ToLowerInvariant(), cancellationToken);
		if(user is not null) {
			error.AddValidationError("Email", "email_is_unavailable", "Email is unavailable.");
		}

		user = await repository.GetByNameAsync(request.UserName.ToLowerInvariant(), cancellationToken);
		if(user is not null) {
			error.AddValidationError("UserName", "username_is_unavailable", "UserName is unavailable.");
		}

		if(error.ValidationErrors.Any()) {
			throw new InvalidCredentialsException()
			{
				Error = error
			};
		}

		var password = passwordHasher.HashPassword(default, request.Password);

		user = new User
		{
			Id = Guid.NewGuid(),
			Name = request.UserName.ToLowerInvariant(),
			Email = request.Email.ToLowerInvariant(),
			Password = password,
			Role = "User",
			Claims = new Dictionary<string, IEnumerable<string>>(),
			CreatedAt = clock.CurrentDate(),
			IsActive = true,
			EmailConfirm = false,
			EmailToConfirm = request.Email.ToLowerInvariant()
		};


		var token = tokenProvider.GenerateConfirmEmailToken(user.Id, user.Email);

		user.EmailConfirmToken = token;

		await repository.AddAsync(user, cancellationToken);

		await CreateEmail(
			request.ConfirmEmailUrl.Replace("__token__", token),
			user.EmailToConfirm,
			cancellationToken);

		return user.Id;
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
