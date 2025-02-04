using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;

namespace MyCar.Module.Users.Api.Controllers;

[Route(UserModule.BasePath)]
// [Authorize]	// Only for users with UserManager claim
internal class HomeController(IUserService service) : HomeControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		return Ok(await service.GetUsers(cancellationToken));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
	{
		return Ok(await service.GetUser(id, cancellationToken));
	}

	[HttpPost("update-privilege")]
	public async Task<IActionResult> UpdatePrivilege([FromBody] UserPrivilegeDto command, CancellationToken cancellationToken)
	{
		await service.UpdateClaims(command, cancellationToken);

		return NoContent();
	}
}
