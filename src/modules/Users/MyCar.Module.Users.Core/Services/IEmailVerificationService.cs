using MyCar.Module.Users.Core.DTO;

namespace MyCar.Module.Users.Core.Services;
public interface IEmailVerificationService
{
	Task SendSample(CancellationToken cancellationToken);
	Task Confirm(ConfirmEmailDto dto, CancellationToken cancellationToken);
}
