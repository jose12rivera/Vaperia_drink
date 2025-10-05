using System.ComponentModel.DataAnnotations;

namespace Vaperia_drink.Models;

public class MetodoPagos
{
    [Key]
    public int MetodoPagoId { get; set; }
    [Required(ErrorMessage = "El nombre del método de pago es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;  // Ej: Efectivo, Tarjeta, Transferencia

    // Relaciones
    public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
}
