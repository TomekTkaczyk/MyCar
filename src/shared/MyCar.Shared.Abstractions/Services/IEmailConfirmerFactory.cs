using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Auth;
public interface IEmailConfirmerFactory
{
	IEmailConfirmer GetEmailConfirmer();
}