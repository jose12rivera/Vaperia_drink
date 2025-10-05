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
    public DbSet<MetodoPagos> MetodosPago { get; set; }
}
