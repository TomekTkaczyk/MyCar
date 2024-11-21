using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;

namespace MyCar.Module.Users.Api.Controllers;


[Route(UserModule.BasePath + "/[controller]")]
internal class AccountController(IIdentityService service) : HomeControllerBase
{
	private readonly IIdentityService _service = service;

	[HttpGet]
	//[Authorize]
	public async Task <ActionResult<AccountDto>> GetAsync()
		=> OkOrNotFound(await _service.GetAsync(Guid.Parse(User.Identity.Name)));

	[HttpPost("sign-up")]
	public async Task <ActionResult> SignUpAsync(SignUpDto dto)
	{
		await _service.SignUpAsync(dto);
		return NoContent();
	}

	[HttpPost("sign-in")]
	public async Task<ActionResult> SignInAsync(SignInDto dto)
		=> Ok(await _service.SignInAsync(dto));
}
