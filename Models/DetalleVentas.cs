namespace Vaperia_drink.Models
{
    public class DetalleVentas
    {
        public int DetalleVentaId { get; set; }

        // FK Venta
        public int VentaId { get; set; }
        public Ventas Venta { get; set; } = default!;

        // FK Producto
        public int ProductoId { get; set; }
        public Productos Producto { get; set; } = default!;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }

        // FK Usuario (quien registró el detalle)
        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; } = default!;
    }
}
