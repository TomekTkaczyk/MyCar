namespace MyCar.Module.Employees.Core.Exceptions;

internal class CannotDeleteEmployeeException(Guid id) : EmployeeException($"Employee wth ID: {id} cannot by deleted.")
{
	public Guid Id { get; } = id;
}
