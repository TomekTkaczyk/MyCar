using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Module.Users.Core.Exceptions;
internal class EmailAlreadyConfirmedException() : MyCarException("Email is already confirmed.", StatusCodes.Status400BadRequest) { }
