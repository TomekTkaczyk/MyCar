﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;
internal static class Extensions
{
	public static IServiceCollection AddErrorHandling(this IServiceCollection services)
		=> services
		.AddScoped<ErrorHandlerMiddleware>()
		.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
		.AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();

	public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
		=> app.UseMiddleware<ErrorHandlerMiddleware>();
}
