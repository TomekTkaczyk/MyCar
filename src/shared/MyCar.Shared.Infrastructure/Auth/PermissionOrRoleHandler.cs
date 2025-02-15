using Microsoft.AspNetCore.Authorization;

namespace MyCar.Shared.Infrastructure.Auth;
internal class PermissionOrRoleHandler : AuthorizationHandler<PermissionOrRoleRequirement>
{
	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionOrRoleRequirement requirement)
	{
		if(context.User == null) {
			return Task.CompletedTask;
		}

		Console.WriteLine($"🔍 Debug: Sprawdzanie dostępu dla użytkownika: {context.User.Identity?.Name}");

		foreach(var claim in context.User.Claims) {
			Console.WriteLine($"    - Claim: {claim.Type} = {claim.Value}");
		}

		if(requirement.RequiredRoles.Any()) {
			var roles = context.User.Claims
				.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
				.Select(c => c.Value.ToLowerInvariant());
			if(roles.Any(r => requirement.RequiredRoles.Contains(r.ToLowerInvariant()))) {
				context.Succeed(requirement);
				return Task.CompletedTask;
			}
		}
		if(requirement.RequiredPermissions.Any()) {
			var permissions = context.User.Claims
				.Where(c => c.Type == "permissions")
				.Select(c => c.Value);
			if(permissions.Any(p => requirement.RequiredPermissions.Contains(p))) {
				context.Succeed(requirement);
				return Task.CompletedTask;
			}
		}
		return Task.CompletedTask;
	}
}
