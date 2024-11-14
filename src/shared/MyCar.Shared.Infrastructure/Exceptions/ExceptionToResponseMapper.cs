using Humanizer;
using MyCar.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyCar.Shared.Infrastructure.Exceptions;
internal class ExceptionToResponseMapper : IExceptionToResponseMapper
{
	private static readonly ConcurrentDictionary<Type, string> _codes = new();

	public ExceptionResponse Map(Exception exception)
		=> exception switch {
			MyCarException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), 
				HttpStatusCode.BadRequest),
			_ => new ExceptionResponse(new Error("error", "There was an error."),
				HttpStatusCode.InternalServerError)
		};

	private record Error(string Code, string Message);

	private record ErrorsResponse(params Error[] Errors);

	private static string GetErrorCode(object exception) {

		var type = exception.GetType();

		return _codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
	}

}
