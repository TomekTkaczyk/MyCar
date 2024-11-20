using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Auth;
public sealed class AuthManager(IClock clock) : IAuthManager
{
	private readonly IClock _clock = clock;

	public JsonWebToken CreateToken(
		string userId, 
		string role = null, 
		string audience = null, 
		IDictionary<string, IEnumerable<string>> claims = null)
	{
		return new JsonWebToken()
		{
			Id = userId,
			Role = role,
			Claims = claims
		};
	}
}
