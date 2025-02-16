using MediatR;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace MyCar.Module.Users.UseCases.Commands.ConfirmEmail;
internal class ConfirmEmailHandler(
	IUserRepository repository,
	IEmailConfirmerFactory emailConfirmerFactory) : IRequestHandler<ConfirmEmailCommand>
{
	public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var confirmToken = DecodeJwt(request.Token);

		var idClaim = confirmToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
			?? throw new InvalidEmailTokenException();

		if(!Guid.TryParse(idClaim, out Guid id)) {
			throw new InvalidEmailTokenException();
		}

		var emailClaim = confirmToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value
			?? throw new InvalidEmailTokenException();

		var user = await repository.GetAsync(id, cancellationToken)
			?? throw new InvalidEmailTokenException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		if(!user.EmailToConfirm.Equals(emailClaim)) {
			throw new InvalidEmailTokenException();
		}

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer(EmailConfirmTypes.Jwt);
		if(!emailConfirmer.Confirm(user.EmailConfirmToken, request.Token, emailClaim)) {
			throw new UserEmailConfirmException();
		}

		user.Email = emailClaim;
		user.EmailConfirm = true;

		await repository.UpdateAsync(user, cancellationToken);
	}

	private static JwtSecurityToken DecodeJwt(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		if(!tokenHandler.CanReadToken(token)) {
			throw new InvalidEmailTokenException();
		}

		return tokenHandler.ReadJwtToken(token);
	}
}
