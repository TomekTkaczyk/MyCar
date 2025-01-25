using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class InvalidCredentialsException() : MyCarException(
	"Invalid credentials.", StatusCodes.Status400BadRequest)
{
}
