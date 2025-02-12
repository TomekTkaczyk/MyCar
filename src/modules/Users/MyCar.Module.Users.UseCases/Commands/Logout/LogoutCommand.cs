using MediatR;

namespace MyCar.Module.Users.UseCases.Commands.Logout;
internal sealed record LogoutCommand(Guid Id) : IRequest { }
