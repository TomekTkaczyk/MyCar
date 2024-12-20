using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MyCar.Shared.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace MyCar.Bootstraper.Controllers;

[ApiController]
[Route("[controller]")]
public class SmtpController(IOptionsMonitor<SmtpOptions> optionsMonitor) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var options = optionsMonitor.CurrentValue;
		await Task.CompletedTask;

		return Ok(JsonSerializer.Serialize(options));
	}
}
