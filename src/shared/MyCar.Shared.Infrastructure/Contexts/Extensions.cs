using Microsoft.Extensions.DependencyInjection;
using MyCar.Shared.Abstractions.Contexts;

namespace MyCar.Shared.Infrastructure.Contexts;

internal static class Extensions
{
	public static IServiceCollection AddContexts(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddSingleton<IContextFactory, ContextFactory>();
		services.AddTransient<IContext>(sp => sp.GetRequiredService<IContextFactory>().Create());

		return services;
	} 
}
