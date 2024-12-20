using Microsoft.AspNetCore.Mvc;

namespace MyCar.Module.Auth.Api.Controllers;


[Route(AuthModule.BasePath)]
internal class HomeController : HomeControllerBase
{
	[HttpGet]
	public ActionResult<string> Get() => "Auth API";
}
