
using Microsoft.AspNetCore.Mvc;

namespace MyCar.Module.Employees.Api.Controllers;

[Route(EmployeeModule.BasePath)]
internal class HomeController : HomeControllerBase
{
	[HttpGet]
	public static ActionResult<string> Get() => "Employees API";
}
