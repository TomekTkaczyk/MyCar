using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class UserNotActiveException(Guid id) : MyCarException($"User with ID: {id} is not active.", StatusCodes.Status400BadRequest) 
{
	public Guid Id { get; } = id;
}
