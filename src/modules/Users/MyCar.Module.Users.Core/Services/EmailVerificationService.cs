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
		//ConfirmToken decode !!! and compliance check !!! 

		var user = await userRepository.GetByEmailToken(dto.ConfirmToken)
			?? throw new InvalidCredentialsException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
		if(!emailConfirmer.Confirm(user.EmailConfirmToken, dto.ConfirmToken, dto.Email)) {
			throw new UserEmailConfirmException();
		}

		user.Email = dto.Email;
		user.EmailConfirm = true;
		user.EmailConfirmToken = null;

		await userRepository.UpdateAsync(user);
	}
}
