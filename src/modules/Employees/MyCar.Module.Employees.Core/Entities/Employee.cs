using MyCar.Shared.Infrastructure.Entities;

namespace MyCar.Module.Employees.Core.Entities;

public class Employee : EntityBase
{
	public string Firstname { get; set; }

	public string Lastname { get; set; }

	public string Description { get; set; }
}
