using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;

namespace MyCar.Shared.Infrastructure.Auth;

public class EmailConfirmerFactory(AuthOptions options, IClock clock) : IEmailConfirmerFactory
{
	public IEmailConfirmer GetEmailConfirmer()
	{
		_ = Enum.TryParse<EmailConfirmTypes>(options.EmailConfirmType, true, out EmailConfirmTypes confirmType);

		return GetEmailConfirmer(confirmType);
	}

	public IEmailConfirmer GetEmailConfirmer(EmailConfirmTypes confirmTypes)
	{
		return confirmTypes switch
		{
			EmailConfirmTypes.Jwt => new JwtEmailConfirmer(options, clock),
			EmailConfirmTypes.Code => new CodeEmailConfirmer(),
			_ => null,
		};
	}
}