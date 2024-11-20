using MyCar.Module.Users.Core.DTO;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.Core.Services;

public interface IIdentityService
{
	Task<AccountDto> GetAsync(Guid id);
	
	Task<JsonWebToken> SignInAsync(SignInDto dto);

	Task SignUpAsync(SignUpDto dto);
}