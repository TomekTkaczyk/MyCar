using Microsoft.EntityFrameworkCore;
using MyCar.Module.Employees.Core.Entities;

namespace MyCar.Module.Employees.Core.DAL;
internal class EmployeesDbContext(
	DbContextOptions<EmployeesDbContext> options) : DbContext(options)
{
	public DbSet<Employee> Employees { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("Employees");
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
