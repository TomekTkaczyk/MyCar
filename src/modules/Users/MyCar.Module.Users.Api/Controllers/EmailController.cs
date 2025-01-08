using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;
using MyCar.Shared.Abstractions.Contexts;

namespace MyCar.Module.Users.Api.Controllers;


[Route(UserModule.BasePath + "/[controller]")]
internal class EmailController(IEmailVerificationService service) : HomeControllerBase
{
	[HttpPost("send")]
	[AllowAnonymous]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> EmailSendAsync(CancellationToken cancellationToken)
	{
		await service.SendSample(cancellationToken);

		return Ok();
	}


}
