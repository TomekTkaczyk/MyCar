using Microsoft.IdentityModel.Tokens;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyCar.Shared.Infrastructure.Auth;
internal class JwEmailConfirmer(AuthOptions options, IClock clock) : IEmailConfirmer
{
	public string ConfirmToken { get; private set; }

	public string GetConfirmEmailBody(Guid userId, string email)
	{
		ConfirmToken = CreateToken(userId, email);

		return ConfirmToken;
	}

	public bool Confirm(string expected, string received)
	{
		throw new NotImplementedException();
	}

	private string CreateToken(Guid userId, string email)
	{
		var now = clock.CurrentDate();
		if(options.EmailConfirmExpiry.TotalSeconds < 1) {
			options.EmailConfirmExpiry = TimeSpan.FromDays(1);
		}
		var expires = now.Add(options.EmailConfirmExpiry);
		var claims = new List<Claim> {
			new("code", GenerateVerificationCode()),
			new("email", email)
		};

		var jwt = new JwtSecurityToken(
			userId.ToString(),
			expires: expires,
			notBefore: now,
			claims: claims);

		var body = new JwtSecurityTokenHandler().WriteToken(jwt);

		return body;
	}

	private static string GenerateVerificationCode()
	{
		var rnd = new Random();
		return rnd.Next(100000, 999999).ToString();
	}
}
