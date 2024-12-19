using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Shared.Infrastructure.Middleware;
internal class RefreshTokenMiddleware(RequestDelegate next, ITokenValidator tokenValidator)
{
	public async Task InvokeAsync(HttpContext context)
	{
		if(context.Request.Path.StartsWithSegments("/users-module/Account/refresh-token")) {
			var refreshToken = context.Request.Cookies["refreshtoken"];
			if(!tokenValidator.GetToken(refreshToken)
				.IsNotExpired()
				.Validate()) {
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Refresh token is missing.");
				return;
			};
		}
		await next(context);
	}
}
