namespace Vaperia_drink.Models
{
    public class Ventas
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // FK Cliente
        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; } = default!;

        // FK Usuario
        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; } = default!;

        public decimal Total { get; set; }

        // Relaciones
        public ICollection<DetalleVentas> Detalles { get; set; } = new List<DetalleVentas>();
        public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
    }
}
