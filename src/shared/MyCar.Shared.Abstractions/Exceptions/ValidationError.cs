namespace MyCar.Shared.Abstractions.Exceptions;
public record ValidationError(
	string Field,
	string Code,
	string Message);
