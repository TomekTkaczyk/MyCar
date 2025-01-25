using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.Core.Services;
internal class UserService(IUserRepository repository) : IUserService
{

	public async Task<IReadOnlyList<UserDto>> GetUsers(CancellationToken cancellationToken = default)
	{
		return await repository.GetAll()
			.Where(x => x.Name.ToLower() != "admin")
			.Select(user => UserDto.Create(user))
			.ToListAsync(cancellationToken);
	}

	public async Task<UserDto> GetUser(Guid id, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(id, cancellationToken);

		return UserDto.Create(user);
	}

	public async Task UpdateClaims(UserPrivilegeDto userClaimsDto, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(userClaimsDto.Id, cancellationToken);
		user.Role = userClaimsDto.Role;
		user.IsActive = userClaimsDto.IsActive;
		user.Claims = userClaimsDto.Claims;

		await repository.UpdateAsync(user, cancellationToken);
	}
}
