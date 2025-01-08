using Microsoft.AspNetCore.Http;

namespace MyCar.Module.Users.Core.Exceptions;

internal class NameIsInUseException() : UserException("Name is already taken.", StatusCodes.Status400BadRequest) { }
