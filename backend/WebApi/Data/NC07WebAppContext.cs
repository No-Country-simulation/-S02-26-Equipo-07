using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data;

public partial class NC07WebAppContext : DbContext
{
    public NC07WebAppContext()
    {
    }

    public NC07WebAppContext(DbContextOptions<NC07WebAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Caballo> Caballos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Jinete> Jinetes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caballo>(entity =>
        {
            entity.ToTable("Caballo");

            entity.Property(e => e.AlturaCm)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AlturaCM");
            entity.Property(e => e.Anchura).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Musculatura)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Raza)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoTorso)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Caballos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Caballo_User");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Jinete>(entity =>
        {
            entity.ToTable("Jinete");

            entity.Property(e => e.AlturaCm)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AlturaCM");
            entity.Property(e => e.AnchoCadera).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Disciplina)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LargoPierna).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Nivel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PesoKg)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("PesoKG");

            entity.HasOne(d => d.User).WithMany(p => p.Jinetes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Jinete_User");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.Property(e => e.CostoUnitario).HasColumnType("money");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descuento)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Iva)
                .HasDefaultValue(2100m)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("IVA");
            entity.Property(e => e.Lote)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SKU");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Categoria)
                .HasConstraintName("FK_Producto_Producto");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("habilitado");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
