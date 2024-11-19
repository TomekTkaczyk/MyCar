using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.DAL;
internal class UsersDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<User> Employees { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("Userss");
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
