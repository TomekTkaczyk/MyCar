using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;

namespace MyCar.Shared.Infrastructure.Services;
internal static class Etensions
{
	public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IEmailService, FakeEmailService>();
		services.AddSingleton<IEmailServiceFactory, EmailServiceFactory>();

		services.AddHostedService<AppInitializer>();
		services.AddHostedService<SmtpEmailProcessingBackgroundService>();

		return services;
	}
}
