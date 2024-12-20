using MyCar.Module.Employees.Core.Entities;

namespace MyCar.Module.Employees.Core.Repositories;

internal interface IEmployeeRepository
{
	Task AddAsync(Employee employee, CancellationToken cancellationToken);

	Task<Employee> GetAsync(Guid id, CancellationToken cancellationToken);

	Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken);

	Task UpdateAsync(Employee employee, CancellationToken cancellationToken);

	Task DeleteAsync(Employee employee, CancellationToken cancellationToken);
}

