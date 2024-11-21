using Microsoft.AspNetCore.Identity;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Auth;
internal class PasswordManager(IPasswordHasher<object> passwordHasher) : IPasswordManager
{
	private readonly IPasswordHasher<object> _passwordHasher = passwordHasher;

	public string Secure(string password)
		=> _passwordHasher.HashPassword(default, password);

	public bool Validate(string password, string securePassword)
		=> _passwordHasher.VerifyHashedPassword(default, password, securePassword) is PasswordVerificationResult.Success;

}
