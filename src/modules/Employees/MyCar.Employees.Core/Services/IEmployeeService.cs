using MyCar.Employees.Core.DTO;

namespace MyCar.Employees.Core.Services;

internal interface IEmployeeService
{
	Task AddAsync(EmployeeDto dto);

	Task<EmployeeDetailsDto> GetAsync(Guid id);
	
	Task<IReadOnlyList<EmployeeDto>> GetAllAsync();
	
	Task UpdateAsync(EmployeeDetailsDto dto);

	Task DeleteAsync(Guid id);
}
