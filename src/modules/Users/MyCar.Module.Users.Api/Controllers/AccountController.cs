using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> SignUpAsync(SignUpDto dto, CancellationToken cancellationToken)
	{
		var frontendUrl = Request.Headers["X-Frontend-Url"].ToString();

		_ = await service.SignUpAsync(dto, frontendUrl, cancellationToken);

		return Created();
	}

	[HttpPost("change-email")]
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangeEmailAsync([FromQuery] string email, CancellationToken cancellationToken)
	{
		var frontendUrl = Request.Headers["X-Frontend-Url"].ToString();

		await service.ChangeEmailAsync(context.Identity.Id, email, frontendUrl, cancellationToken);
		return NoContent();
	}

	[HttpPost("sign-in")]
	[AllowAnonymous]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> SignInAsync(SignInDto dto, CancellationToken cancellationToken)
	{
		var response = await service.SignInAsync(dto, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			SameSite = SameSiteMode.Strict,
		};
		Response.Cookies.Append("accessToken", response.AccessToken, cookieOptions);
		Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

		return Ok();
	}

	[HttpPost("refresh-token")]
	[AllowAnonymous]
	[ProducesResponseType(204)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
	{
		var refreshToken = Request.Cookies["refreshtoken"];

		var response = await service.RefreshTokenAsync(refreshToken, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			SameSite = SameSiteMode.Strict,
		};
		Response.Cookies.Append("accessToken", response.AccessToken, cookieOptions);
		Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

		return NoContent();
	}

	[HttpPost("logout")]
	[ProducesResponseType(204)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> Logout(CancellationToken cancellationToken)
	{
		await service.LogoutAsync(context.Identity.Id, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			SameSite = SameSiteMode.Strict,
			Expires = DateTime.UtcNow.AddDays(-1),
		};
		Response.Cookies.Append("accessToken", "", cookieOptions);
		Response.Cookies.Append("refreshToken", "", cookieOptions);

		return NoContent();
	}


	[HttpPost("remaind-password")]
	[AllowAnonymous]
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> RemaindPasswordAsync([FromQuery] string email, CancellationToken cancellationToken)
	{
		var frontendUrl = Request.Headers["X-Frontend-Url"].ToString();

		await service.RemaindPasswordAsync(email, frontendUrl, cancellationToken);
		return NoContent();
	}


	[HttpPost("change-password")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken cancellationToken)
	{
		await service.ChangePasswordAsync(context.Identity.Id, dto, cancellationToken);
		return NoContent();
	}


	[HttpPut("update-profile")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> UpdateProfileAsync(UserProfileDto dto, CancellationToken cancellationToken)
	{
		await service.UpdateProfileAsync(context.Identity.Id, dto, cancellationToken);

		return NoContent();
	}

	[HttpPost("confirm-email")]
	[AllowAnonymous]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> ConfirmEmailAsync([FromQuery] string token, CancellationToken cancellationToken)
	{
		await service.ConfirmEmailAsync(token, cancellationToken);
		return Ok();
	}

	[HttpPost("resend-confirm-email")]
	[AllowAnonymous]
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> ResendConfirmEmailTokenAsync(string email, CancellationToken cancellationToken)
	{
		var frontendUrl = Request.Headers["X-Frontend-Url"].ToString();

		await service.ResendConfirmEmailTokenAsync(email, frontendUrl, cancellationToken);
		return Ok();
	}

	[HttpGet("get-permissions")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	public async Task<ActionResult<IEnumerable<string>>> GetPermissionsAsync(CancellationToken cancellationToken)
	{
		var permissions = await service.GetAllPermissionsAsync(cancellationToken);
		return Ok(permissions);
	}
}
