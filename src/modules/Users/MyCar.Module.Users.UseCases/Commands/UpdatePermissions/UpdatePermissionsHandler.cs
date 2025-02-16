﻿using MediatR;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.UseCases.Commands.UpdatePermissions;
internal class UpdatePermissionsHandler(IUserRepository repository) : IRequestHandler<UpdatePermissionsCommand>
{
	public async Task Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.GetAsync(request.Id, cancellationToken);
		user.Role = request.Role;
		user.IsActive = request.IsActive;
		user.Claims = request.Permissions;

		await repository.UpdateAsync(user, cancellationToken);
	}
}
