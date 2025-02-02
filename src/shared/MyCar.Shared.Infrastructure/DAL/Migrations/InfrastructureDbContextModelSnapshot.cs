﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCar.Shared.Infrastructure.DAL;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyCar.Shared.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(InfrastructureDbContext))]
    partial class InfrastructureDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Infrastructure")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyCar.Shared.Infrastructure.Entities.StoredFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileDescription")
                        .HasColumnType("text");

                    b.Property<string>("FileHash")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileStorageName")
                        .HasColumnType("text");

                    b.Property<string>("FileStoragePath")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FileStorageName")
                        .IsUnique();

                    b.ToTable("StoredFiles", "Infrastructure");
                });
#pragma warning restore 612, 618
        }
    }
}
