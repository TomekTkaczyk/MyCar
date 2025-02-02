using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCar.Shared.Abstractions.Entities;
using System.Text.Json;

namespace MyCar.Shared.Infrastructure.DAL.Configurations;
internal class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	public void Configure(EntityTypeBuilder<StoredFile> builder)
	{
		builder.HasIndex(x => x.FileStorageName).IsUnique();
	}
}
