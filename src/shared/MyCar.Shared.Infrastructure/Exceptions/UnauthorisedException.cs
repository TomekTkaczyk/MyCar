using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;

public sealed class UnauthorisedException() : MyCarException("Unauthorise.", StatusCodes.Status401Unauthorized) { }
