using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Module.Users.Core;
using MyCar.Module.Users.Core.Services;
using MyCar.Shared.Abstractions.Modules;

namespace MyCar.Module.Users.Api;

internal class UserModule : IModule
{
	public const string BasePath = "/users-module";

	public string Name { get; } = "Users";

	public string Path => BasePath;

	public IEnumerable<string> Policies { get; } = ["UserManager", "UserUprawnienie1", "UserUprawnienie2"];

	public void Register(IServiceCollection services, IConfiguration configuration)
	{
		services.AddCore(configuration);


		services.AddMediatR(cfg =>
		{
			var assemblies = AppDomain.CurrentDomain
			.GetAssemblies()
			.Where(x => x.GetName().Name.StartsWith("MyCar.Module.Users.", StringComparison.CurrentCultureIgnoreCase))
			.ToArray();
			cfg.RegisterServicesFromAssemblies(assemblies);
		});

		//services.Scan(scan => scan
		//	.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
		//	.AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
		//	.AsImplementedInterfaces()
		//	.WithScopedLifetime()
		//);
		//services.Scan(scan => scan
		//	.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
		//	.AddClasses(classes => classes.AssignableTo(typeof(IRequest)))
		//	.AsImplementedInterfaces()
		//	.WithScopedLifetime()
		//);
	}

	public void Use(IApplicationBuilder app)
	{
		using(var scope = app.ApplicationServices.CreateScope()) {
			scope.ServiceProvider
				.GetRequiredService<IDataInitializerService>()
				.Initialize()
				.Wait();
		}
	}
}
