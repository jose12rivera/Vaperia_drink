using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vaperia_drink.Models;

public class Usuarios
{
    [Key]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre completo no puede superar los 100 caracteres.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    [RegularExpression("vendedor|admin", ErrorMessage = "El rol debe ser 'vendedor' o 'admin'.")]
    public string Rol { get; set; } = "vendedor"; // vendedor / admin

    public string? FotoUrl { get; set; }
    public byte[]? Foto { get; set; }

    // Relaciones
    public ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
    public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
}
