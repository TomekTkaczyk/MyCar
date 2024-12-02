namespace MyCar.Shared.Abstractions.Auth;

public interface ITokenProvider
{
	string GenerateAccessToken(Guid userId, string role, IDictionary<string, IEnumerable<string>> claims);
	RefreshToken GenerateRefreshToken();
}
