using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Infrastructure.Auth;

namespace MyCar.Module.Users.Core.Services;
internal class EmailVerificationService(
	IUserRepository userRepository,
	IEmailConfirmerFactory emailConfirmerFactory) : IEmailVerificationService
{
	public async Task Confirm(ConfirmEmailDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByEmailAsync(dto.Email)
			?? throw new InvalidCredentialException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
		if(!emailConfirmer.Confirm(user.EmailConfirmToken, dto.ConfirmToken)) {
			throw new UserEmailConfirmException();
		}

		user.EmailConfirm = true;
		user.EmailConfirmToken = null;

		await userRepository.UpdateAsync(user);
	}
}
