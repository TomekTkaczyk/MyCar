using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Employees.Core;
using MyCar.Shared.Abstractions.Modules;

namespace MyCar.Employees.Api;
internal class EmployeeModule : IModule
{
	public const string BasePath = "employees-module";

	public string Name { get; } = "Employees";

	public string Path => BasePath;

	public void Register(IServiceCollection services)
	{
		using(var scope = services.BuildServiceProvider()) {
			var configuration = scope.GetRequiredService<IConfiguration>();
			services.AddCore(configuration);
		}
	}

	public void Use(IApplicationBuilder app)
	{
	}
}
