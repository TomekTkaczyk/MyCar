using MediatR;
using MyCar.Module.Users.Core.DAL.Repositories;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Infrastructure.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace MyCar.Module.Users.UseCases.Commands.RefreshToken;
internal class RefreshTokenHandler(
	IUserRepository repository,
	ITokenProvider tokenProvider,
	IClock clock) : IRequestHandler<RefreshTokenCommand, JsonWebToken>
{
	public async Task<JsonWebToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(request.Token);
		var sub = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
		var exp = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
		if(exp != null && long.TryParse(exp, out var expUnixSecons)) {
			var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnixSecons).UtcDateTime;
			var now = clock.CurrentDate();
			if(expirationDate < now) {
				throw new UnauthorisedException();
			}
		}
		else {
			throw new UnauthorisedException();
		};

		var user = await repository.GetAsync(new Guid(sub), cancellationToken)
			?? throw new UnauthorisedException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		if(!user.RefreshToken.Equals(request.Token)) {
			throw new UnauthorisedException();
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
