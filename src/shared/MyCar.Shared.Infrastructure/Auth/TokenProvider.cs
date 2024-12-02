using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyCar.Shared.Abstractions;
using System.Security.Claims;
using System.Text;
using MyCar.Shared.Abstractions.Auth;
using System.Security.Cryptography;

namespace MyCar.Shared.Infrastructure.Auth;

internal sealed class TokenProvider(AuthOptions options, IClock clock) : ITokenProvider
{
	public string GenerateAccessToken(Guid userId, string role, IDictionary<string, IEnumerable<string>> claims)
	{
		var secretKey = options.IssuerSigningKey;

		var now = clock.CurrentDate();

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(
			[
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString()),
				new Claim("role", role)
			]),
			Issuer = "MyCar",
			Expires = now.Add(options.Expiry),
			SigningCredentials = credentials,
		};

		var handler = new JsonWebTokenHandler();
		var token = handler.CreateToken(tokenDescriptor);

		return token;
	}

	public RefreshToken GenerateRefreshToken()
	{
		var now = clock.CurrentDate();
		var refreshToken = new RefreshToken(
			Token: Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
			CreatedAt: now,
			Expires: now.Add(options.RefreshExpiry));

		return refreshToken;
	}
}
