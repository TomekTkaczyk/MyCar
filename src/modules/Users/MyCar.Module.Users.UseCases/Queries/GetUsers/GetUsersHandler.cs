using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Queries.GetUsers;
internal class GetUsersHandler(IUserRepository repository) : IRequestHandler<GetUsersQuery, IReadOnlyList<UserProfileDto>>
{
	public async Task<IReadOnlyList<UserProfileDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
	{
		return await repository.GetAll()
			.Where(x => !x.Name.Equals("Admin"))
			.Select(user => UserProfileDto.Create(user))
			.ToListAsync(cancellationToken);
	}
}
