namespace Vaperia_drink.Models
{
    public class Inventarios
    {
        public int InventarioId { get; set; }

        // FK Producto
        public int ProductoId { get; set; }
        public Productos Producto { get; set; } = default!;

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;
        public int Cantidad { get; set; }

        // tipoMovimiento: "Entrada", "Salida"
        public string TipoMovimiento { get; set; } = string.Empty;
    }
}
