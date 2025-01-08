using Humanizer;

namespace MyCar.Shared.Abstractions.Exceptions;

public abstract class MyCarException : Exception
{
	private string ErrorCode => GetType().Name.Underscore().Replace("_exception", string.Empty).ToLowerInvariant();

	public ApiError Error { get; init; }

	public MyCarException(string message, int status) : base(message)
	{
		Error = new ApiError()
		{
			Message = message,
			Code = ErrorCode,
			Status = status
		};
	}
}
