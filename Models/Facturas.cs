namespace Vaperia_drink.Models
{
    public class Facturas
    {
        public int FacturaId { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        // FK Venta
        public int VentaId { get; set; }
        public Ventas Venta { get; set; } = default!;

        // FK Cliente
        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; } = default!;

        // FK Usuario
        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; } = default!;

        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }

        // FK Método de Pago
        public int MetodoPagoId { get; set; }
        public MetodoPagos MetodoPago { get; set; } = default!;
    }
}
