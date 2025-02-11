using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Queries.GetUsers;
internal class GetUsersHandler(IUserRepository repository) : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
	public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
	{
		return await repository.GetAll()
			.Where(x => !x.Name.ToLower().Equals("admin"))
			.Select(user => UserDto.Create(user))
			.ToListAsync(cancellationToken);
	}
}
