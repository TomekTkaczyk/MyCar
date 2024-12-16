using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class InvalidCredentialsException() : MyCarException("Invalid credentials.") { }
