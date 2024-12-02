using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Auth;
internal class CodeEmailConfirmer() : IEmailConfirmer
{
	public string ConfirmToken { get; private set; }

	public string GetConfirmEmailBody(Guid userId, string email)
	{
		ConfirmToken = GenerateVerificationCode();

		return $"Confirm code: {ConfirmToken}";
	}

	private static string GenerateVerificationCode()
	{
		var rnd = new Random();
		return rnd.Next(100000, 999999).ToString();
	}

	public bool Confirm(string expected, string reciverd) => expected.Equals(reciverd, StringComparison.InvariantCultureIgnoreCase);
}
