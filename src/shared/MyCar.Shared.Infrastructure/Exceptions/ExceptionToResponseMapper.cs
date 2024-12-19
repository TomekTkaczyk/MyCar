using Humanizer;
using MyCar.Shared.Abstractions.Exceptions;
using System.Collections.Concurrent;
using System.Net;

namespace MyCar.Shared.Infrastructure.Exceptions;
internal class ExceptionToResponseMapper : IExceptionToResponseMapper
{
	private static readonly ConcurrentDictionary<Type, string> _codes = new();

	public ExceptionResponse Map(Exception exception)
		=> exception switch {
			MyCarException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), 
				GetStatusCode(ex)),
			_ => new ExceptionResponse(new Error("error", "There was an error."),
				HttpStatusCode.InternalServerError)
		};

	private record Error(string Code, string Message);

	private record ErrorsResponse(params Error[] Errors);

	private static string GetErrorCode(object exception) {

		var type = exception.GetType();

		return _codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
	}

	private static HttpStatusCode GetStatusCode(MyCarException exception)
		=> exception.GetType().Name switch
		{
			"UserNotActiveException" => HttpStatusCode.Unauthorized,
			"UnauthorisedException" => HttpStatusCode.Unauthorized,
			_ => HttpStatusCode.BadRequest
		};

}
