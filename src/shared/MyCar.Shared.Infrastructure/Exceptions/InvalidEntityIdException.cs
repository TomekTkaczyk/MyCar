using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;

public sealed class InvalidEntityIdException(object id) : MyCarException($"Cannot set: {id} as entity identifier.", StatusCodes.Status400BadRequest)
{
	public object Id { get; } = id;
}
