using MediatR;
using MyCar.Module.Users.Core.DTO;

namespace MyCar.Module.Users.UseCases.Queries.GetUser;
internal sealed record GetUserQuery(Guid Id) : IRequest<UserDto> { }
