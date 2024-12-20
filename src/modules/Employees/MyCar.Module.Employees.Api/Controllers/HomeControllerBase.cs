using Microsoft.AspNetCore.Mvc;
using MyCar.Shared.Infrastructure.Api;

namespace MyCar.Module.Employees.Api.Controllers;

[ApiController]
[Route(EmployeeModule.BasePath)]
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
