using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Employees.Core.Exceptions;

public abstract class EmployeeException(string message, int status) : MyCarException(message, status) { }
