using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Employees.Core.DAL;
using MyCar.Employees.Core.DAL.Repositories;
using MyCar.Employees.Core.Policies;
using MyCar.Employees.Core.Repositories;
using MyCar.Employees.Core.Services;
using MyCar.Shared.Infrastructure.Database;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyCar.Employees.Api")]

namespace MyCar.Employees.Core;

internal static class Extensions
{
	public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) {

		services.AddDatatabase<EmployeesDbContext>(configuration);
		//services.AddSingleton<IEmployeeRepository, EmployeeRepositoryInMemory>();
		services.AddScoped<IEmployeeRepository, EmployeeRepository>();
		services.AddSingleton<IEmployeeDeletionPolicy, EmployeeDeletionPolicy>();
		services.AddScoped<IEmployeeService, EmployeeService>();

		return services;
	}
}
