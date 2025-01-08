using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Abstractions.Services;
public interface IEmailConfirmerFactory
{
	IEmailConfirmer GetEmailConfirmer();
	IEmailConfirmer GetEmailConfirmer(EmailConfirmTypes confirmTypes);
}