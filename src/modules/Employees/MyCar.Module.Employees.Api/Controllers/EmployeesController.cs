using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Employees.Core.DTO;
using MyCar.Module.Employees.Core.Services;

namespace MyCar.Module.Employees.Api.Controllers;


[Route(EmployeeModule.BasePath + "/[controller]")]
internal class EmployeesController(IEmployeeService service) : HomeControllerBase
{

	[HttpGet("{id:Guid}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult<EmployeeDetailsDto>> GetAsync(Guid id, CancellationToken cancellationToken)
	{
		return Ok(await service.GetAsync(id, cancellationToken));
	}

	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult<EmployeeDetailsDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		return Ok(await service.GetAllAsync(cancellationToken));
	}

	[HttpPost]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	[ProducesResponseType(403)]
	public async Task<ActionResult> AddAsync(EmployeeDto dto, CancellationToken cancellationToken)
	{
		await service.AddAsync(dto, cancellationToken);

		return CreatedAtAction("Get", new { id = dto.Id }, null);
	}

	[HttpPut]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	[ProducesResponseType(403)]
	public async Task<ActionResult> UpdateAsync(EmployeeDetailsDto dto, CancellationToken cancellationToken)
	{
		await service.UpdateAsync(dto, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id:Guid}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(401)]
	[ProducesResponseType(403)]
	public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		await service.DeleteAsync(id, cancellationToken);

		return NoContent();
	}
}
