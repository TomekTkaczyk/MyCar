using MyCar.Module.Users.Core.DTO;

namespace MyCar.Module.Users.Core.Services;
internal interface IUserService
{
	Task<IReadOnlyList<UserDto>> GetUsers(CancellationToken cancellationToken);
	Task<UserDto> GetUser(Guid id, CancellationToken cancellationToken);
	Task UpdateClaims(UserPrivilegeDto userClaimsDto, CancellationToken cancellationToken);
}
