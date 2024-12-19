using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCar.Module.Auth.Core;
using MyCar.Shared.Abstractions.Modules;
using System.Reflection;

namespace MyCar.Module.Auth.Api;

internal class AuthModule : IModule
{
	public const string BasePath = "auth-module";

	public string Name { get; } = "Auth";

	public string Path => BasePath;

	public IEnumerable<string> Policies { get; } = ["auth"];

	public void Register(IServiceCollection services, IConfiguration configuration)
	{
		services.AddCore(configuration);
	}

	public void Use(IApplicationBuilder app)
	{
	}
}
