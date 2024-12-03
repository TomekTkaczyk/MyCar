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
			Expires = response.RefreshToken.Expires,
		};
		Response.Cookies.Append("refreshToken", response.RefreshToken.Token, cookieOptions);

		return Ok(response.AccesToken);
	}

	[HttpPost("refresh-token")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
	{
		var refreshToken = Request.Cookies["refresh-token"];
		
		var response = await service.RefreshTokenAsync(context.Identity.Id, refreshToken, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = response.RefreshToken.Expires,
		};
		Response.Cookies.Append("refreshToken", response.RefreshToken.Token, cookieOptions);

		return Ok(response.AccesToken);
	}

	[HttpPost("logout")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> Logout(CancellationToken cancellationToken)
	{
		await service.Logout(context.Identity.Id, cancellationToken);

		return Ok();
	}

	[HttpGet("forgot-password")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
	{
		await service.ForgotPassword(email, cancellationToken);
		return NoContent();
	}
}
