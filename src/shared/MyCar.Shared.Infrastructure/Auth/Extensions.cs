using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Modules;

namespace MyCar.Shared.Infrastructure.Auth;
public static class Extensions
{
	public static IServiceCollection AddAuth(
		this IServiceCollection services,
		IConfiguration configuration,
		IList<IModule> modules,
		Action<JwtBearerOptions> optionsFactory = null)
	{
		var options = configuration.GetOptions<AuthOptions>("auth");

		services.AddSingleton<IPasswordManager, PasswordManager>();
		services.AddSingleton<IAuthManager, AuthManager>();

		return services;
	}
}
