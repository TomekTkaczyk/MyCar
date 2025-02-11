using MediatR;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Commands.UpdateClaims;
internal class UpdateClaimsHandler(IUserRepository repository) : IRequestHandler<UpdateClaimsCommand>
{
	public async Task Handle(UpdateClaimsCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(request.Id, cancellationToken);
		user.Role = request.Role;
		user.IsActive = request.IsActive;
		user.Claims = request.Claims;

		await repository.UpdateAsync(user, cancellationToken);
	}
}
