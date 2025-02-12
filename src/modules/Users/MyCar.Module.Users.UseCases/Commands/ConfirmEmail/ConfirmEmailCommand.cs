using MediatR;

namespace MyCar.Module.Users.UseCases.Commands.ConfirmEmail;
internal sealed record ConfirmEmailCommand(string Token) : IRequest { }
