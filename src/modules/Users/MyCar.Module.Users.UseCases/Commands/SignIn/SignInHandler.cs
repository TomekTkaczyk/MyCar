using MediatR;
using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.UseCases.Commands.SignIn;
internal class SignInHandler(
	IUserRepository repository,
	IPasswordHasher<User> passwordHasher,
	ITokenProvider tokenProvider) : IRequestHandler<SignInCommand, JsonWebToken>
{
	private const string privilegeName = "privileges";

	public async Task<JsonWebToken> Handle(SignInCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetByIdentifierAsync(request.Identifier.ToLowerInvariant(), cancellationToken)
		?? throw new InvalidCredentialsException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, request.Password) is not PasswordVerificationResult.Success) {
			throw new InvalidCredentialsException();
		}

		var claims = new Dictionary<string, IEnumerable<string>>
		{
			{ "permissions", user.GetPermissions() }
		};

		var jwt = new JsonWebToken
		{
			AccessToken = tokenProvider.GenerateAccessToken(user.Id, user.Role, claims),
			RefreshToken = tokenProvider.GenerateRefreshToken(user.Id),
		};

		user.RefreshToken = jwt.RefreshToken;
		await repository.UpdateAsync(user, cancellationToken);

		return jwt;
	}
}
