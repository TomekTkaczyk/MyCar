﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCar.Module.Employees.Core.DTO;
using MyCar.Module.Employees.Core.Services;
using System.Threading;

namespace MyCar.Module.Employees.Api.Controllers;


[Route(EmployeeModule.BasePath + "/[controller]")]
internal class EmployeesController(
	IEmployeeService service) : HomeControllerBase
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

	[HttpPost("upload-file")]
	public async Task<IActionResult> UploadExcelFile(List<IFormFile> file, CancellationToken cancellationToken)
	{
		var size = file.Sum(f => f.Length);
		var guids = new List<Guid>();

		foreach(var formFile in file) {
			if(formFile.Length > 0) {
				guids.Add(await service.AddFileAsync(formFile, cancellationToken));
			}
		}

		return Ok(new { count = file.Count, size, guids });
	}


}
