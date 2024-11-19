using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Module.Users.Core;
using MyCar.Shared.Abstractions.Modules;

namespace MyCar.Module.Users.Api;
internal class UserModule : IModule
{
	public const string BasePath = "users-module";

	public string Name { get; } = "Users";

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
