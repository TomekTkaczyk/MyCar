using MyCar.Employees.Core.Entities;

namespace MyCar.Employees.Core.Policies;
internal interface IEmployeeDeletionPolicy
{
	Task<bool> CanDeleteAsync(Employee employee);
}
