using Microsoft.AspNetCore.Http;

namespace MyCar.Module.Users.Core.Exceptions;

internal class EmailIsInUseException() : UserException("Email is already taken.", StatusCodes.Status400BadRequest) { }
