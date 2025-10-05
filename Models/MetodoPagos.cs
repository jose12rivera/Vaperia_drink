namespace Vaperia_drink.Models
{
    public class MetodoPagos
    {
        public int MetodoPagoId { get; set; }
        public string Nombre { get; set; } = string.Empty; // Ej: Efectivo, Tarjeta, Transferencia

        // Relaciones
        public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
    }
}
