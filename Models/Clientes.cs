namespace Vaperia_drink.Models
{
    public class Clientes
    {
        public int ClienteId { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Relaciones
        public ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
        public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();

    }
}
