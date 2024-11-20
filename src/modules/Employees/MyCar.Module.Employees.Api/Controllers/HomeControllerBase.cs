using Microsoft.AspNetCore.Mvc;

namespace MyCar.Module.Employees.Api.Controllers;

[ApiController]
[Route(EmployeeModule.BasePath)]
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
