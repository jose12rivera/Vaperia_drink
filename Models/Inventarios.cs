using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaperia_drink.Models;

public class Inventarios
{
    [Key]
    public int InventarioId { get; set; }

    // FK Producto
    public int ProductoId { get; set; }
    [ForeignKey("ProductoId")]
    public Productos Producto { get; set; } = default!;

    [Required(ErrorMessage = "La fecha del movimiento es obligatoria.")]
    public DateTime FechaMovimiento { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
    [RegularExpression("Entrada|Salida", ErrorMessage = "El tipo de movimiento debe ser 'Entrada' o 'Salida'.")]
    public string TipoMovimiento { get; set; } = string.Empty;
}
