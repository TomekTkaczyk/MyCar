using Microsoft.AspNetCore.Mvc;
using MyCar.Employees.Core.DTO;
using MyCar.Employees.Core.Services;

namespace MyCar.Employees.Api.Controllers;


[Route(EmployeeModule.BasePath + "/[controller]")]
internal class EmployeesController(IEmployeeService service) : HomeControllerBase
{
	private readonly IEmployeeService _employeeService = service;

	[HttpGet("{id:Guid}")]
	public async Task<ActionResult<EmployeeDetailsDto>> GetAsync(Guid id)
	{
		return Ok(await _employeeService.GetAsync(id));
	}

	[HttpGet]
	public async Task<ActionResult<EmployeeDetailsDto>> GetAllAsync()
	{
		return Ok(await _employeeService.GetAllAsync());
	}

	[HttpPost]
	public async Task<ActionResult> AddAsync(EmployeeDto dto)
	{
		await _employeeService.AddAsync(dto);

		return CreatedAtAction("Get", new { id = dto.Id }, null);
	}

	[HttpPatch]
	public async Task<ActionResult> UpdateAsync(EmployeeDetailsDto dto)
	{
		await _employeeService.UpdateAsync(dto);

		return NoContent();
	}

	[HttpDelete("{id:Guid}")]
	public async Task<ActionResult> DeleteAsync(Guid id)
	{
		await _employeeService.DeleteAsync(id);

		return NoContent();
	}
}
