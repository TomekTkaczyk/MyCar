using Microsoft.AspNetCore.Mvc;
using MyCar.Shared.Infrastructure.Api;

namespace MyCar.Module.Users.Api.Controllers;

[ApiController]
[Route(UserModule.BasePath)]
[ProducesDefaultContentType]
internal abstract class HomeControllerBase : ControllerBase
{
	protected ActionResult<T> OkOrNotFound<T>(T model)
	{
		if(model is null) {
			return NotFound();
		}

		return Ok(model);
	}
}
