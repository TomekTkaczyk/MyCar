using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;
using MyCar.Shared.Abstractions.Contexts;

namespace MyCar.Module.Users.Api.Controllers;


[Route(UserModule.BasePath + "/[controller]")]
[Authorize]
internal class AccountController(
	IIdentityService service,
	IContext context) : HomeControllerBase
{
	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<ActionResult<AccountDto>> GetAsync(CancellationToken cancellationToken)
	{
		return OkOrNotFound(await service.GetAsync(context.Identity.Id, cancellationToken));
	}

	[HttpPost("sign-up")]
	[AllowAnonymous]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> SignUpAsync(SignUpDto dto, CancellationToken cancellationToken)
	{
		_= await service.SignUpAsync(dto, cancellationToken);

		return NoContent();
	}

	[HttpPost("sign-in")]
	[AllowAnonymous]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> SignInAsync(SignInDto dto, CancellationToken cancellationToken)
	{
		var response = await service.SignInAsync(dto, cancellationToken);
		
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
		};
		Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

		return Ok(response.AccesToken);
	}

	[HttpGet("refresh-token")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
	{
		var refreshToken = Request.Cookies["refresh-token"];
		
		var response = await service.RefreshTokenAsync(context.Identity.Id, refreshToken, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
		};
		Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

		return Ok(response.AccesToken);
	}

	[HttpGet("logout")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> Logout(CancellationToken cancellationToken)
	{
		await service.LogoutAsync(context.Identity.Id, cancellationToken);

		return Ok();
	}


	[HttpPut("forgot-password")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
	{
		await service.ForgotPasswordAsync(email, cancellationToken);
		return Ok();
	}


	[HttpPut("change-password")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken)
	{
		await service.ChangePasswordAsync(context.Identity.Id, dto, cancellationToken);
		return NoContent();
	}


	[HttpPut("change-email")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangeEmailAsync(ChangeEmailDto dto, CancellationToken cancellationToken)
	{
		await service.ChangeEmailAsync(context.Identity.Id, dto, cancellationToken);
		return NoContent();
	}


	[HttpPut("update-profile")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken)
	{
		await service.UpdateProfileAsync(context.Identity.Id, dto, cancellationToken);

		return NoContent();
	}
}
