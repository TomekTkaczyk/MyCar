using MyCar.Shared.Abstractions.Modules;
using MyCar.Shared.Infrastructure;
using MyCar.Shared.Infrastructure.Modules;
using MyCar.Shared.Infrastructure.Services;
using System.Reflection;

namespace MyCar.Bootstraper;

public class Program
{
	public static void Main(string[] args)
	{
		IList<Assembly> _assemblies;
		IList<IModule> _modules;

		var builder = WebApplication.CreateBuilder(args);

		var configuration = builder.Configuration;
		var services = builder.Services;

		builder.Host.ConfigureModules();

		_assemblies = ModuleLoader.LoadAssemblies(configuration);
		_modules = ModuleLoader.LoadModules(_assemblies);

		builder.Services.AddInfrastructure(configuration, _modules);

		foreach(var module in _modules) {
			module.Register(services, configuration);
		}

		builder.Services.AddControllers();
		builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("smtp"));


		var app = builder.Build();

		app.Logger.LogInformation("Modules: {Modules}", string.Join(",", _modules.Select(m => m.Name)));

		app.UseInfrastructure(app.Environment);
		foreach(var module in _modules) {
			module.Use(app);
		}

		app.MapControllers();

		app.MapGet("/", context => context.Response.WriteAsync("MyCar API."));
		app.MapGet("modules", context =>
		{
			var moduleInfoProvider = context.RequestServices.GetRequiredService<ModuleInfoProvider>();
			return context.Response.WriteAsJsonAsync(moduleInfoProvider);
		});

		//app.MapGet("email", context =>
		//{
		//	var confirmer = context.RequestServices
		//		.GetRequiredService<EmailConfirmerFactory>()
		//		.GetEmailConfirmer();
		//	var body = confirmer.GetConfirmEmailBody(Guid.NewGuid(), "biuro@unipromax.pl");
		//	var email = new Email
		//	{
		//		Body = body,
		//		Subject = "Sample activating your account in the MyCar application",
		//		Recievers = ["biuro@unipromax.pl"]
		//	};
		//	EmailsQueue.Add(email);
		//	return context.Response.WriteAsync("MyCar API.");
		//});

		_assemblies.Clear();
		_modules.Clear();

		var path = configuration.GetSection("StoredFilePath:path");
		var temp = Path.Combine(path.Value, Path.GetTempFileName());
		Console.WriteLine(temp);

		app.Run();
	}
}
