﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Users.UseCases.Commands.UpdateClaims;
using MyCar.Module.Users.UseCases.Queries.GetUser;
using MyCar.Module.Users.UseCases.Queries.GetUsers;

namespace MyCar.Module.Users.Api.Controllers;

[Route(UserModule.BasePath)]
[Authorize(Policy ="Users.UserManager.OrAdmin")]
internal class HomeController(IMediator mediator) : HomeControllerBase
{
	private readonly IMediator mediator = mediator;

	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		return Ok(await mediator.Send(new GetUsersQuery(), cancellationToken));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
	{
		return Ok(await mediator.Send(new GetUserQuery(id), cancellationToken));
	}

	[HttpPost("update-privilege")]
	public async Task<IActionResult> UpdatePrivilege([FromBody] UpdateClaimsCommand command, CancellationToken cancellationToken)
	{
		await mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
