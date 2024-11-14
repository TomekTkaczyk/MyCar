using Microsoft.EntityFrameworkCore;
using MyCar.Employees.Core.Entities;

namespace MyCar.Employees.Core.DAL;
internal class EmployeesDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Employee> Employees { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.HasDefaultSchema("Employees");
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
