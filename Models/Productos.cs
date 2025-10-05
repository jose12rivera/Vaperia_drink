using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vaperia_drink.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50, ErrorMessage = "La categoría no puede superar los 50 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    public string? FotoUrl { get; set; }
    public byte[]? Foto { get; set; }

    // Relaciones
    public ICollection<DetalleVentas> DetallesVenta { get; set; } = new List<DetalleVentas>();
    public ICollection<Inventarios> MovimientosInventario { get; set; } = new List<Inventarios>();
}
