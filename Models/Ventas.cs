using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaperia_drink.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }
    [Required(ErrorMessage = "La fecha de la venta es obligatoria.")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    // FK Cliente
    public int ClienteId { get; set; }
    [ForeignKey("ClienteId")]
    public Clientes Cliente { get; set; } = default!;

    // FK Usuario
    public int UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuarios Usuario { get; set; } = default!;

    [Required(ErrorMessage = "El total es obligatorio.")]
    [Range(0.0, double.MaxValue, ErrorMessage = "El total no puede ser negativo.")]
    public decimal Total { get; set; }

    // Relaciones
    public ICollection<DetalleVentas> Detalles { get; set; } = new List<DetalleVentas>();
    public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
}
