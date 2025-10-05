namespace Vaperia_drink.Models
{
    public class EstadisticaVentas
    {
        public int EstadisticaVentaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int CantidadVentas { get; set; }
        public decimal TotalVendido { get; set; }
        public int ProductosVendidos { get; set; }

        // Para reportes de inventario
        public int Disminuye { get; set; }
        public int Aumenta { get; set; }
    }
}
