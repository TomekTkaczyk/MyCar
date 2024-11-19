using MyCar.Module.Employees.Core.DTO;

namespace MyCar.Module.Employees.Core.Services;

internal interface IEmployeeService
{
	Task AddAsync(EmployeeDto dto);

	Task<EmployeeDetailsDto> GetAsync(Guid id);

	Task<IReadOnlyList<EmployeeDto>> GetAllAsync();

	Task UpdateAsync(EmployeeDetailsDto dto);

	Task DeleteAsync(Guid id);
}
