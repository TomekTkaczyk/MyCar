using Microsoft.AspNetCore.Http;

namespace MyCar.Module.Users.Core.Exceptions;

internal class UserEmailConfirmException() : UserException("Invalid email confirm token.", StatusCodes.Status400BadRequest) { }
