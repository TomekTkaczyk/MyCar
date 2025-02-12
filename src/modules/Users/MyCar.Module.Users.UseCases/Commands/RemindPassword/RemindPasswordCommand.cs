using MediatR;

namespace MyCar.Module.Users.UseCases.Commands.RemindPassword;
internal sealed record RemindPasswordCommand : IRequest
{
	public string Email { get; init; }

	public string ResetPasswordUrl { get; init; }
}
