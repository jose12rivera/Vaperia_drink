using System;
using System.ComponentModel.DataAnnotations;

namespace Vaperia_drink.Models;

public class EstadisticaVentas
{
    [Key]
    public int EstadisticaVentaId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de ventas no puede ser negativa.")]
    public int CantidadVentas { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "El total vendido no puede ser negativo.")]
    public decimal TotalVendido { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los productos vendidos no pueden ser negativos.")]
    public int ProductosVendidos { get; set; }

    // Para reportes de inventario
    [Range(0, int.MaxValue, ErrorMessage = "El valor de disminución no puede ser negativo.")]
    public int Disminuye { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El valor de aumento no puede ser negativo.")]
    public int Aumenta { get; set; }
}
