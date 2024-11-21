using MyCar.Shared.Abstractions.Modules;
using MyCar.Shared.Infrastructure;
using System.Reflection;
using MyCar.Shared.Infrastructure.Modules;

namespace MyCar.Bootstraper;

public class Program
{
	public static void Main(string[] args)
	{
		IList<Assembly> _assemblies;
		IList<IModule> _modules;

		var builder = WebApplication.CreateBuilder(args);
		var config  =builder.Configuration;
		var services = builder.Services;

		builder.Host.ConfigureModules();

		_assemblies = ModuleLoader.LoadAssemblies(config);
		_modules = ModuleLoader.LoadModules(_assemblies);

		foreach (var module in _modules) {
			module.Register(services);
		}

		builder.Services.AddInfrastructure(config, _modules);

		var app = builder.Build();


		app.UseInfrastructure(app.Environment);
		foreach(var module in _modules) {
			module.Use(app);
		}

		app.MapControllers();
		app.MapGet("/", context => context.Response.WriteAsync("MyCar API."));

		_assemblies.Clear();
		_modules.Clear();

		app.Run();
	}
}
