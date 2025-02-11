using MediatR;
using MyCar.Module.Users.Core.DTO;

namespace MyCar.Module.Users.UseCases.Queries.GetUsers;
internal class GetUsersQuery : IRequest<IReadOnlyList<UserDto>> { }
