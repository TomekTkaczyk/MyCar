using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Module.Employees.Core.DAL;
using MyCar.Module.Employees.Core.DAL.Repositories;
using MyCar.Module.Employees.Core.Policies;
using MyCar.Module.Employees.Core.Repositories;
using MyCar.Module.Employees.Core.Services;
using MyCar.Shared.Infrastructure.Database;
using System.Runtime.CompilerServices;

namespace MyCar.Module.Employees.Core;

internal static class Extensions
{
	public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDatatabase<EmployeesDbContext>(configuration);
		services.AddScoped<IEmployeeRepository, EmployeeRepository>();
		services.AddSingleton<IEmployeeDeletionPolicy, EmployeeDeletionPolicy>();
		services.AddScoped<IEmployeeService, EmployeeService>();

		return services;
	}
}
