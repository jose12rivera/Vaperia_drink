namespace Vaperia_drink.Models
{
    public class Productos
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public string? FotoUrl { get; set; }
        public byte[]? Foto { get; set; }

        // Relaciones
        public ICollection<DetalleVentas> DetallesVenta { get; set; } = new List<DetalleVentas>();
        public ICollection<Inventarios> MovimientosInventario { get; set; } = new List<Inventarios>();
    }
}
