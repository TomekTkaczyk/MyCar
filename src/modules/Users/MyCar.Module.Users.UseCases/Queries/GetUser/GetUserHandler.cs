using MediatR;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Queries.GetUser;
internal class GetUserHandler(IUserRepository repository) : IRequestHandler<GetUserQuery, UserDto>
{
	public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(request.Id, cancellationToken);

		return UserDto.Create(user);
	}
}
