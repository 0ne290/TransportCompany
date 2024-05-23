using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Dal;

public partial class TransportCompanyContext : DbContext
{
    public TransportCompanyContext()
    {
    }

    public TransportCompanyContext(DbContextOptions<TransportCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=TransportCompany;uid=root;pwd=\"!IgEcA21435=\"", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PRIMARY");

            entity.ToTable("driver");

            entity.Property(e => e.Guid).HasMaxLength(36);
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.TruckNumber, "TruckNumber_idx");

            entity.HasIndex(e => e.UserLogin, "UserLogin_idx");

            entity.Property(e => e.Guid).HasMaxLength(36);
            entity.Property(e => e.Address).HasMaxLength(45);
            entity.Property(e => e.CargoVolume).HasPrecision(56, 28);
            entity.Property(e => e.CargoWeight).HasPrecision(56, 28);
            entity.Property(e => e.DateBegin).HasColumnType("datetime");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.Price).HasPrecision(56, 28);
            entity.Property(e => e.TruckNumber).HasMaxLength(9);
            entity.Property(e => e.UserLogin).HasMaxLength(45);

            entity.HasOne(d => d.TruckNumberNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TruckNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TruckNumber");

            entity.HasOne(d => d.UserLoginNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserLogin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserLogin");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PRIMARY");

            entity.ToTable("truck");

            entity.HasIndex(e => e.DriverGuid, "DriverGuid_UNIQUE").IsUnique();

            entity.Property(e => e.Number).HasMaxLength(9);
            entity.Property(e => e.DriverGuid).HasMaxLength(36);
            entity.Property(e => e.TypeAdr).HasMaxLength(45);
            entity.Property(e => e.VolumeMax).HasPrecision(56, 28);
            entity.Property(e => e.WeightMax).HasPrecision(56, 28);

            entity.HasOne(d => d.Driver).WithOne(p => p.Truck)
                .HasForeignKey<Truck>(d => d.DriverGuid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("DriverGuid");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Password, "Password_UNIQUE").IsUnique();

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Contact).HasMaxLength(45);
            entity.Property(e => e.DefaultAddress).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
