using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;
using MyCar.Shared.Abstractions.Contexts;

namespace MyCar.Module.Users.Api.Controllers;


[Route(UserModule.BasePath + "/[controller]")]
internal class EmailController(IEmailVerificationService service) : HomeControllerBase
{
	[HttpGet("email-confirm")]
	[AllowAnonymous]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> EmailConfirmAsync([FromQuery] ConfirmEmailDto dto, CancellationToken cancellationToken)
	{
		await service.Confirm(dto, cancellationToken);

		return Ok();
	}
}
