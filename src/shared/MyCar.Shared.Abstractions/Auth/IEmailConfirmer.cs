namespace MyCar.Shared.Abstractions.Auth;

public interface IEmailConfirmer
{
	string ConfirmToken { get; }
	string GetConfirmToken(Guid userId, string email);
	string GetConfirmEmailBody(Guid userId, string email, string confirmUrl);
	string GetRemaindPasswordBody(Guid userId, string email);
	bool Confirm(string expected, string received, string email);
}
