using Microsoft.AspNetCore.Mvc;

namespace MyCar.Module.Users.Api.Controllers;

[Route(UserModule.BasePath)]
internal class HomeController : HomeControllerBase
{
	[HttpGet]
	public static ActionResult<string> Get() => "Users API";
}
