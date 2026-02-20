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

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<MovimientoStock> MovimientosStock { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<DetalleVenta> DetallesVenta { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<NotaCredito> NotasCredito { get; set; }

    public virtual DbSet<NotaDebito> NotasDebito { get; set; }

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

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stock");

            entity.Property(e => e.ProductoId);
            entity.Property(e => e.CantidadDisponible);
            entity.Property(e => e.CantidadReservada);
            entity.Property(e => e.CantidadMinima);
            entity.Property(e => e.UltimaActualizacion).HasColumnType("datetime");

            entity.HasOne(d => d.Producto).WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Producto");
        });

        modelBuilder.Entity<MovimientoStock>(entity =>
        {
            entity.ToTable("MovimientoStock");

            entity.Property(e => e.StockId);
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cantidad);
            entity.Property(e => e.Razon)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaMovimiento).HasColumnType("datetime");
            entity.Property(e => e.UserId);

            entity.HasOne(d => d.Stock).WithMany(p => p.MovimientosStock)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoStock_Stock");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_MovimientoStock_User");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Venta");

            entity.Property(e => e.UserId);
            entity.Property(e => e.FechaVenta).HasColumnType("datetime");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.Descuento).HasColumnType("money");
            entity.Property(e => e.IVA).HasColumnType("money");
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Ventas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_User");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable("DetalleVenta");

            entity.Property(e => e.VentaId);
            entity.Property(e => e.ProductoId);
            entity.Property(e => e.Cantidad);
            entity.Property(e => e.PrecioUnitario).HasColumnType("money");
            entity.Property(e => e.Descuento).HasColumnType("money");
            entity.Property(e => e.Subtotal).HasColumnType("money");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Venta");

            entity.HasOne(d => d.Producto).WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Producto");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("Pago");

            entity.Property(e => e.VentaId);
            entity.Property(e => e.Monto).HasColumnType("money");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenciaPago)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaConfirmacion).HasColumnType("datetime");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId);

            entity.HasOne(d => d.Venta).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Venta");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Pago_User");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.ToTable("Factura");

            entity.Property(e => e.VentaId);
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoFactura)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaEmision).HasColumnType("datetime");
            entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CUIT)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.CUITCliente)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumentoCliente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumentoCliente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CondicionIva)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CAE)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.FechaVencimientoCae).HasColumnType("datetime");
            entity.Property(e => e.Subtotal).HasColumnType("money");
            entity.Property(e => e.Descuento).HasColumnType("money");
            entity.Property(e => e.IVA).HasColumnType("money");
            entity.Property(e => e.PorcentajeIVA).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IIBB)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

            entity.HasOne(d => d.Venta).WithMany()
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Venta");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Factura_User");
        });

        modelBuilder.Entity<NotaCredito>(entity =>
        {
            entity.ToTable("NotaCredito");

            entity.Property(e => e.FacturaId);
            entity.Property(e => e.NumeroNotaCredito)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNotaCredito)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaEmision).HasColumnType("datetime");
            entity.Property(e => e.Razon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CAE)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.FechaVencimientoCae).HasColumnType("datetime");
            entity.Property(e => e.CUITCliente)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("money");
            entity.Property(e => e.IVA).HasColumnType("money");
            entity.Property(e => e.PorcentajeIVA).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IIBB)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.UserId);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

            entity.HasOne(d => d.Factura).WithMany()
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_Factura");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_NotaCredito_User");
        });

        modelBuilder.Entity<NotaDebito>(entity =>
        {
            entity.ToTable("NotaDebito");

            entity.Property(e => e.FacturaId);
            entity.Property(e => e.NumeroNotaDebito)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNotaDebito)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaEmision).HasColumnType("datetime");
            entity.Property(e => e.Razon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CAE)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.FechaVencimientoCae).HasColumnType("datetime");
            entity.Property(e => e.CUITCliente)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("money");
            entity.Property(e => e.IVA).HasColumnType("money");
            entity.Property(e => e.PorcentajeIVA).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IIBB)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.UserId);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

            entity.HasOne(d => d.Factura).WithMany()
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_Factura");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_NotaDebito_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
