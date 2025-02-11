using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Module.Employees.Core;
using MyCar.Shared.Abstractions.Modules;

namespace MyCar.Module.Employees.Api;
internal class EmployeeModule : IModule
{
	public const string BasePath = "employees-module";

	public string Name { get; } = "Employees";

	public string Path => BasePath;

	public IEnumerable<string> Policies { get; } = ["employees", "users"];

	public void Register(IServiceCollection services, IConfiguration configuration)
	{
		services.AddCore(configuration);

		services.AddMediatR(cfg =>
		{
			var assemblies = AppDomain.CurrentDomain
			.GetAssemblies()
			.Where(x => x.GetName().Name.StartsWith("MyCar.Module.Employees.", StringComparison.CurrentCultureIgnoreCase))
			.ToArray();
			cfg.RegisterServicesFromAssemblies(assemblies);
		});
	}

	public void Use(IApplicationBuilder app)
	{
	}
}
