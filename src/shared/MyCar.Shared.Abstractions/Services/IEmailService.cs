namespace MyCar.Shared.Abstractions.Services;
public interface IEmailService
{
	Task<bool> SendEmail(Email email, int counter);
}
