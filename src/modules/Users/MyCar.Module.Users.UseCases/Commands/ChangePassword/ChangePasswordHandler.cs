using MediatR;
using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Commands.ChangePassword;
internal class ChangePasswordHandler(
	IUserRepository repository,
	IPasswordHasher<User> passwordHasher) : IRequestHandler<ChangePasswordCommand>
{
	public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(request.Id, cancellationToken)
		?? throw new InvalidCredentialsException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, request.CurrentPassword) is not PasswordVerificationResult.Success) {
			throw new InvalidPasswordException();
		}

		var password = passwordHasher.HashPassword(default, request.Password);
		user.Password = password;

		await repository.UpdateAsync(user, cancellationToken);
	}
}
