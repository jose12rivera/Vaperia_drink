using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vaperia_drink.Models;

namespace Vaperia_drink.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Productos> Productos { get; set; }
    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<Ventas> Ventas { get; set; }
    public DbSet<DetalleVentas> DetalleVentas { get; set; }
    public DbSet<Inventarios> Inventarios { get; set; }
    public DbSet<EstadisticaVentas> EstadisticaVentas { get; set; }
    public DbSet<Facturas> Facturas { get; set; }
    public DbSet<MetodoPagos> MetodosPagos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar claves primarias explícitamente
        modelBuilder.Entity<Clientes>()
            .HasKey(c => c.ClienteId);

        modelBuilder.Entity<Productos>()
            .HasKey(p => p.ProductoId);

        modelBuilder.Entity<Usuarios>()
            .HasKey(u => u.UsuarioId);

        modelBuilder.Entity<Ventas>()
            .HasKey(v => v.VentaId);

        modelBuilder.Entity<DetalleVentas>()
            .HasKey(d => d.DetalleVentaId);

        modelBuilder.Entity<Inventarios>()
            .HasKey(i => i.InventarioId);

        modelBuilder.Entity<EstadisticaVentas>()
            .HasKey(e => e.EstadisticaVentaId);

        modelBuilder.Entity<Facturas>()
            .HasKey(f => f.FacturaId);

        modelBuilder.Entity<MetodoPagos>()
            .HasKey(m => m.MetodoPagoId);

        // Configurar relaciones
        modelBuilder.Entity<Ventas>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ventas>()
            .HasOne(v => v.Usuario)
            .WithMany(u => u.Ventas)
            .HasForeignKey(v => v.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleVentas>()
            .HasOne(d => d.Venta)
            .WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DetalleVentas>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesVenta)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleVentas>()
            .HasOne(d => d.Usuario)
            .WithMany()
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventarios>()
            .HasOne(i => i.Producto)
            .WithMany(p => p.MovimientosInventario)
            .HasForeignKey(i => i.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Facturas>()
            .HasOne(f => f.Venta)
            .WithMany(v => v.Facturas)
            .HasForeignKey(f => f.VentaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facturas>()
            .HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facturas>()
            .HasOne(f => f.Usuario)
            .WithMany(u => u.Facturas)
            .HasForeignKey(f => f.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facturas>()
            .HasOne(f => f.MetodoPago)
            .WithMany(m => m.Facturas)
            .HasForeignKey(f => f.MetodoPagoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurar propiedades decimales para mayor precisión
        modelBuilder.Entity<Productos>()
            .Property(p => p.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleVentas>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleVentas>()
            .Property(d => d.SubTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Ventas>()
            .Property(v => v.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Facturas>()
            .Property(f => f.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Facturas>()
            .Property(f => f.Impuestos)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Facturas>()
            .Property(f => f.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<EstadisticaVentas>()
            .Property(e => e.TotalVendido)
            .HasPrecision(18, 2);
    }
}