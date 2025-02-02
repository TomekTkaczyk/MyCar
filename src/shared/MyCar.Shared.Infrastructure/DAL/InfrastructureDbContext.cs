using Microsoft.EntityFrameworkCore;
using MyCar.Shared.Abstractions.Entities;

namespace MyCar.Shared.Infrastructure.DAL;
internal class InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options) : DbContext(options)
{
	public DbSet<StoredFile> StoredFiles { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("Infrastructure");
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
