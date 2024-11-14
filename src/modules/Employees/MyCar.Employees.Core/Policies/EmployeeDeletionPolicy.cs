using MyCar.Employees.Core.Entities;

namespace MyCar.Employees.Core.Policies;

internal class EmployeeDeletionPolicy : IEmployeeDeletionPolicy
{

	public async Task<bool> CanDeleteAsync(Employee employee) {

		await Task.CompletedTask;

		return true;
	}
}
