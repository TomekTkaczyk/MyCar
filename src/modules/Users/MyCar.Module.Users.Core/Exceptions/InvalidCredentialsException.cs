using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class InvalidCredentialsException : MyCarException 
{
	public InvalidCredentialsException() : base("Invalid credentials.", StatusCodes.Status400BadRequest) { }
}
