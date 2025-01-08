using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;
public sealed class InvalidEmailTokenException() : MyCarException("Invalid email token.", StatusCodes.Status401Unauthorized) { }
