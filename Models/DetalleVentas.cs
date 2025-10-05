using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaperia_drink.Models;

public class DetalleVentas
{
    [Key]
    public int DetalleVentaId { get; set; }

    // FK Venta
    public int VentaId { get; set; }
    [ForeignKey("VentaId ")]
    public Ventas Venta { get; set; } = default!;

    // FK Producto
    public int ProductoId { get; set; }
    [ForeignKey("ProductoId")]
    public Productos Producto { get; set; } = default!;

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El precio unitario es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que 0.")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "El subtotal es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor que 0.")]
    public decimal SubTotal { get; set; }

    // FK Usuario (quien registró el detalle)
    public int UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuarios Usuario { get; set; } = default!;
}
