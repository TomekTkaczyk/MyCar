namespace MyCar.Module.Employees.Core.Exceptions;
internal class EmployeeNotFoundException(Guid id) : EmployeeException($"Employee wth ID: {id} was not found.")
{
	public Guid Id { get; } = id;
}
