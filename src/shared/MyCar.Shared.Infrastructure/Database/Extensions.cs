using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCar.Shared.Infrastructure.Database;
public static class Extensions
{
	private const string _dbSectionName = "Postgress";

	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) {

		var dbOptions = configuration.GetOptions<DatabaseOptions>(_dbSectionName);
		services.AddSingleton(dbOptions);

		return services;
	}

	public static IServiceCollection AddDatatabase<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext {
		var options = configuration.GetOptions<DatabaseOptions>(_dbSectionName);
		services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));

		return services;
	}
}
