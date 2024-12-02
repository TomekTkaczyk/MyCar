namespace MyCar.Shared.Abstractions.Auth;

public interface IEmailConfirmer
{
	string ConfirmToken { get; }
	string GetConfirmEmailBody(Guid userId, string email);
	bool Confirm(string expected, string received);
}
