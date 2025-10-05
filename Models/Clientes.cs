using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vaperia_drink.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;


    // Relaciones
    public ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
    public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
}
