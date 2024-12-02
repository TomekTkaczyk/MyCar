using MyCar.Module.Users.Core.DTO;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.Core.Services;

public interface IIdentityService
{
	Task<AccountDto> GetAsync(Guid id, CancellationToken cancellationToken);
	Task<JsonWebToken> SignInAsync(SignInDto dto, CancellationToken cancellationToken);
	Task<JsonWebToken> RefreshTokenAsync(Guid id, string token, CancellationToken cancellationToken);
	Task<Guid> SignUpAsync(SignUpDto dto, CancellationToken cancellationToken);
	Task ConfirmEmail(ConfirmEmailDto dto, CancellationToken cancellationToken);
	Task ForgotPassword(string email, CancellationToken cancellationToken);
	Task Logout(Guid id, CancellationToken cancellationToken);
}