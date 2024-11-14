using MyCar.Employees.Core.Entities;

namespace MyCar.Employees.Core.Repositories;

internal class EmployeeRepositoryInMemory : IEmployeeRepository
{

	private readonly List<Employee> _employees = [];

	public Task AddAsync(Employee employee)
	{

		_employees.Add(employee);

		return Task.CompletedTask;
	}

	public async Task<Employee> GetAsync(Guid id)
	{

		await Task.CompletedTask;

		return _employees.SingleOrDefault(emp => emp.Id == id);
	}

	public async Task<IReadOnlyList<Employee>> GetAllAsync()
	{

		await Task.CompletedTask;

		return _employees.ToList();
	}

	public Task DeleteAsync(Employee employee)
	{

		_employees.Remove(employee);

		return Task.CompletedTask;
	}

	public Task UpdateAsync(Employee employee)
	{

		return Task.CompletedTask;
	}
}

