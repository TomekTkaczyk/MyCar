using MediatR;

namespace MyCar.Module.Users.UseCases.Commands.ChangeEmail;
internal sealed record ChangeEmailCommand : IRequest
{
	public Guid Id { get; init; }
	public string Email { get; init; }
	public string ConfirmEmailUrl { get; init; }
}
