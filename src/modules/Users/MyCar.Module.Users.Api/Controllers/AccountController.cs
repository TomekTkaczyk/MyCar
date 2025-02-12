﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Services;
using MyCar.Module.Users.UseCases.Commands.ChangeEmail;
using MyCar.Module.Users.UseCases.Commands.ChangePassword;
using MyCar.Module.Users.UseCases.Commands.ConfirmEmail;
using MyCar.Module.Users.UseCases.Commands.Logout;
using MyCar.Module.Users.UseCases.Commands.RefreshToken;
using MyCar.Module.Users.UseCases.Commands.RemindPassword;
using MyCar.Module.Users.UseCases.Commands.SignIn;
using MyCar.Module.Users.UseCases.Commands.SignUp;
using MyCar.Module.Users.UseCases.Commands.UpdateName;
using MyCar.Module.Users.UseCases.Queries.GetUser;
using MyCar.Shared.Abstractions.Contexts;

namespace MyCar.Module.Users.Api.Controllers;


[Route(UserModule.BasePath + "/[controller]")]
[Authorize]

internal class AccountController(
	IMediator mediator,
	IContext context,
	IHttpContextAccessor httpContextAccessor) : HomeControllerBase
{
	private readonly IMediator mediator = mediator;
	private readonly IContext context = context;
	private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<ActionResult<UserDto>> GetAsync(CancellationToken cancellationToken)
	{
		return OkOrNotFound(await mediator.Send(new GetUserQuery(context.Identity.Id), cancellationToken));
	}


	[HttpPost("sign-in")]
	[AllowAnonymous]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> SignInAsync(SignInCommand command, CancellationToken cancellationToken)
	{
		var response = await mediator.Send(command, cancellationToken);
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			SameSite = SameSiteMode.Strict,
		};
		Response.Cookies.Append("accessToken", response.AccessToken, cookieOptions);
		Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

		return Ok();
	}


	[HttpPost("sign-up")]
	[AllowAnonymous]
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken)
	{
		var confirmEmailUrl = Request.Headers["X-Confirmemail-Url"].ToString();
		if(confirmEmailUrl.IsNullOrEmpty()) {
			confirmEmailUrl = Url.Action(
				"ConfirmEmail",
				ControllerContext.ActionDescriptor.ControllerName,
				new { token = @"__token__" },
				httpContextAccessor.HttpContext.Request.Scheme
			);
		} else {
			confirmEmailUrl += @"?token=__token__";
		}

		var command = new SignUpCommand() {
			UserName = request.UserName,
			Email = request.Email,
			Password = request.Password,
			ConfirmEmailUrl = confirmEmailUrl
		};

		await mediator.Send(command, cancellationToken);

		return Created();
	}


	[HttpPost("change-email")]
	[EnableCors("cors-fronturl-header")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangeEmailAsync([FromQuery] string email, CancellationToken cancellationToken)
	{
		var confirmEmailUrl = Request.Headers["X-Confirmemail-Url"].ToString();

		if(confirmEmailUrl.IsNullOrEmpty()) {
			confirmEmailUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme);
		}

		var command = new ChangeEmailCommand()
		{
			Id = context.Identity.Id,
			Email = email,
			ConfirmEmailUrl = confirmEmailUrl
		};

		await mediator.Send(command, cancellationToken);

		return NoContent();
	}



	[HttpPost("refresh-token")]
	[AllowAnonymous]
	[ProducesResponseType(204)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
	{
		var refreshToken = Request.Cookies["refreshtoken"];
		var command = new RefreshTokenCommand(refreshToken);

		var jwt = await mediator.Send(command, cancellationToken);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			SameSite = SameSiteMode.Strict,
		};
		Response.Cookies.Append("accessToken", jwt.AccessToken, cookieOptions);
		Response.Cookies.Append("refreshToken", jwt.RefreshToken, cookieOptions);

		return NoContent();
	}


	[HttpPost("logout")]
	[ProducesResponseType(204)]
	[ProducesResponseType(401)]
	public async Task<ActionResult> Logout(CancellationToken cancellationToken)
	{

		await mediator.Send(new LogoutCommand(context.Identity.Id), cancellationToken);

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


	[HttpPost("remind-password")]
	[EnableCors("cors-fronturl-header")]
	[AllowAnonymous]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> RemindPasswordAsync([FromQuery] string email, CancellationToken cancellationToken)
	{
		var resetPasswordUrl = Request.Headers["X-Frontend-Url"].ToString();

		await mediator.Send(new RemindPasswordCommand() {
			Email = email,
			ResetPasswordUrl = resetPasswordUrl}, cancellationToken);

		return NoContent();
	}


	[HttpPost("change-password")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
	{
		var command = new ChangePasswordCommand()
		{
			Id = context.Identity.Id,
			CurrentPassword = request.CurrentPassword,
			Password = request.Password
		};
		await mediator.Send(command, cancellationToken);

		return NoContent();
	}


	[HttpPut("update-name")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> UpdateNameAsync(UpdateNameRequest request, CancellationToken cancellationToken)
	{
		var command = new UpdateNameCommand()
		{
			Id = context.Identity.Id,
			FirstName = request.FirstName,
			LastName = request.LastName
		};
		await mediator.Send(command, cancellationToken);

		return NoContent();
	}


	[HttpGet("confirm-email")]
	[AllowAnonymous]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> ConfirmEmailAsync([FromQuery] string token, CancellationToken cancellationToken)
	{
		await mediator.Send(new ConfirmEmailCommand(token), cancellationToken);

		return Ok();
	}
}
