using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class UserNotActiveException(Guid id) : MyCarException($"User with ID: {id} is not active.") 
{
	public Guid Id { get; } = id;
}
