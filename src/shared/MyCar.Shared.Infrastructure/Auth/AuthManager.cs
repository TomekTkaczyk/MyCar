using Microsoft.IdentityModel.Tokens;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCar.Shared.Infrastructure.Auth;
public sealed class AuthManager : IAuthManager
{
	private static readonly Dictionary<string, IEnumerable<string>> _emptyClaims = [];
	private readonly AuthOptions _options;
	private readonly IClock _clock;
	private readonly SigningCredentials _signingCredentials;
	private readonly string _issuer;

	public AuthManager(AuthOptions options, IClock clock)
	{
		_ = options.IssuerSigningKey
			?? throw new InvalidOperationException("Issuer signing key is not set.");

		_options = options;
		_clock = clock;
		_signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)),
			SecurityAlgorithms.HmacSha256);
		_issuer = options.Issuer;
	}

	public JsonWebToken CreateToken(
			string userId,
			string role = null,
			string audience = null,
			IDictionary<string, IEnumerable<string>> claims = null)
	{
		if(string.IsNullOrWhiteSpace(userId)) {
			throw new ArgumentNullException(nameof(userId), "User ID claim (subject) cannot by empty.");
		}

		var now = _clock.CurrentDate();
		var jwtClaims = new List<Claim>
		{
		  new(JwtRegisteredClaimNames.Sub, userId),
		  new(JwtRegisteredClaimNames.UniqueName, userId),
		  new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		  new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString()),
		};

		if(!string.IsNullOrEmpty(role)) {
			jwtClaims.Add(new Claim(ClaimTypes.Role, role));
		}

		if(!string.IsNullOrEmpty(audience)) {
			jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
		}

		if(claims?.Any() is true) {
			var customClaims = new List<Claim>();
			foreach(var (claim, values) in claims) {
				customClaims.AddRange(values.Select(value => new Claim(claim, value)));
			}

			jwtClaims.AddRange(customClaims);
		}

		var expires = now.Add(_options.Expiry);

		var jwt = new JwtSecurityToken(
			_issuer,
			claims: jwtClaims,
			notBefore: now,
			expires: expires,
			signingCredentials: _signingCredentials);

		var token = new JwtSecurityTokenHandler().WriteToken(jwt);

		return new JsonWebToken()
		{
			AccesToken = token,
			RefreshToken = string.Empty,
			Expires = new DateTimeOffset(expires).ToUnixTimeMilliseconds(),
			Id = userId,
			Role = role ?? string.Empty,
			Claims = claims ?? _emptyClaims
		};
	}
}
