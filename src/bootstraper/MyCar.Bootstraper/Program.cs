using MyCar.Shared.Abstractions.Modules;
using MyCar.Shared.Infrastructure;
using System.Reflection;

namespace MyCar.Bootstraper;

public class Program
{
	public static void Main(string[] args)
	{
		IList<Assembly> _assemblies = ModuleLoader.LoadAssemblies();
		IList<IModule> _modules = ModuleLoader.LoadModules(_assemblies);

		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddInfrastructure(builder.Configuration);

		foreach (var module in _modules) {
			module.Register(builder.Services);
		}

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
