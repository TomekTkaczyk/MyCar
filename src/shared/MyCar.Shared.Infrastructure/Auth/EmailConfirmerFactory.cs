using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Auth;

public class EmailConfirmerFactory(AuthOptions options, IClock clock) : IEmailConfirmerFactory
{
	public IEmailConfirmer GetEmailConfirmer()
	{
		_ = Enum.TryParse<EmailConfirmTypes>(options.EmailConfirmType, true, out EmailConfirmTypes confirmType);
		return confirmType switch
		{
			EmailConfirmTypes.Jwt => new JwtEmailConfirmer(options, clock),
			EmailConfirmTypes.Code => new CodeEmailConfirmer(),
			_ => null,
		};
	}
}