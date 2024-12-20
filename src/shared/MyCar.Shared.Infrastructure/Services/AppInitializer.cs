using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyCar.Shared.Infrastructure.Services;
internal class AppInitializer(IServiceProvider serviceProvider, ILogger<AppInitializer> logger) : IHostedService
{
	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly ILogger<AppInitializer> _logger = logger;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsInterface && x != typeof(DbContext));

		using(var scope = _serviceProvider.CreateScope()) {
			foreach(var dbType in dbContextTypes) {
				var dbContext = scope.ServiceProvider.GetService(dbType) as DbContext;
				if(dbContext is null) {
					continue;
				}
				await dbContext.Database.MigrateAsync(cancellationToken);
				_logger.LogInformation("Database migration {DbContext}", dbContext.GetType().Name.Replace("DbContext", ""));
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
