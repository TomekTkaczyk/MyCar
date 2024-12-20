using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;

public abstract class UserException(string message) : MyCarException(message) { }
