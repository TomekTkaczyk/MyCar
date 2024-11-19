using MyCar.Module.Employees.Core.Entities;

namespace MyCar.Module.Employees.Core.Repositories;

internal interface IEmployeeRepository
{
	Task AddAsync(Employee employee);

	Task<Employee> GetAsync(Guid id);

	Task<IReadOnlyList<Employee>> GetAllAsync();

	Task UpdateAsync(Employee employee);

	Task DeleteAsync(Employee employee);
}

