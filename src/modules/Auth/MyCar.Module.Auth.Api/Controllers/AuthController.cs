using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCar.Module.Auth.Api.Controllers;
[Route(AuthModule.BasePath + "/[controller]")]
[Authorize]
internal class AuthController : HomeControllerBase
{
}
