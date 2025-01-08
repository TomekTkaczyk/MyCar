using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class InvalidPasswordException() : MyCarException("Invalid password.", StatusCodes.Status400BadRequest) { }

