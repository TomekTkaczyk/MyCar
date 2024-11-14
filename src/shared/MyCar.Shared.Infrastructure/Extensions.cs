using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Infrastructure.Api;
using MyCar.Shared.Infrastructure.Database;
using MyCar.Shared.Infrastructure.Exceptions;
using MyCar.Shared.Infrastructure.Services;
using MyCar.Shared.Infrastructure.Time;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyCar.Bootstraper")]

namespace MyCar.Shared.Infrastructure;

public static class Extensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{

		services.AddDatabase(configuration);
		services.AddErrorHandling();
		services.AddHostedService<AppInitializer>();

		services.AddSingleton<IClock, UtcClock>();

		services.AddControllers()
			.ConfigureApplicationPartManager(manager =>
			{
				manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
			});
		;
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}

	public static IApplicationBuilder UseInfrastructure(
		this IApplicationBuilder app,
		IWebHostEnvironment environment)
	{

		if(environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseErrorHandling();
		app.UseRouting();

		return app;
	}

	public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
	{

		var options = new T();
		configuration.GetSection(sectionName).Bind(options);

		return options;
	}
}
