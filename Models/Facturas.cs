using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaperia_drink.Models;

public class Facturas
{
    [Key]
    public int FacturaId { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.Now;

    // FK Venta
    public int VentaId { get; set; }
    [ForeignKey("VentaId")]
    public Ventas Venta { get; set; } = default!;

    // FK Cliente
    public int ClienteId { get; set; }
    [ForeignKey("ClienteId")]
    public Clientes Cliente { get; set; } = default!;

    // FK Usuario
    public int UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuarios Usuario { get; set; } = default!;

    [Required(ErrorMessage = "El número de factura es obligatorio.")]
    [StringLength(50, ErrorMessage = "El número de factura no puede superar los 50 caracteres.")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Range(0.0, double.MaxValue, ErrorMessage = "El subtotal no puede ser negativo.")]
    public decimal Subtotal { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Los impuestos no pueden ser negativos.")]
    public decimal Impuestos { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "El total no puede ser negativo.")]
    public decimal Total { get; set; }

    // FK Método de Pago
    public int MetodoPagoId { get; set; }
    public MetodoPagos MetodoPago { get; set; } = default!;
}
