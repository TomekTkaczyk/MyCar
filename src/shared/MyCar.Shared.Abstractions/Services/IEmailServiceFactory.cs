namespace MyCar.Shared.Abstractions.Services;

public interface IEmailServiceFactory
{
	IEmailService GetService();
}
