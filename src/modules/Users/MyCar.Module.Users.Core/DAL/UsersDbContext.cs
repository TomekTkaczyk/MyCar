using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.DAL;
internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("Users");
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
