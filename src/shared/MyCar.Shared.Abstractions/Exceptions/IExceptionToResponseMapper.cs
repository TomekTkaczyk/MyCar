namespace MyCar.Shared.Abstractions.Exceptions;
public interface IExceptionToResponseMapper
{
	ExceptionResponse Map(Exception exception);
}
