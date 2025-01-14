using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class EmailNotChanged() : MyCarException(
	"Email has not been changed.", StatusCodes.Status400BadRequest) { }
