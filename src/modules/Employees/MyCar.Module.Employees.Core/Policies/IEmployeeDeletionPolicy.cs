using MyCar.Module.Employees.Core.Entities;

namespace MyCar.Module.Employees.Core.Policies;
internal interface IEmployeeDeletionPolicy
{
	Task<bool> CanDeleteAsync(Employee employee);
}
