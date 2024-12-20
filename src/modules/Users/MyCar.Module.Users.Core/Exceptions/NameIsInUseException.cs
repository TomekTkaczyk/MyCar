namespace MyCar.Module.Users.Core.Exceptions;

internal class NameIsInUseException() : UserException("Name is already taken.") { }
