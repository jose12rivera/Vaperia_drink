namespace Vaperia_drink.Models
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Rol { get; set; } = "vendedor"; // vendedor / admin

        public string? FotoUrl { get; set; }
        public byte[]? Foto { get; set; }

        // Relaciones
        public ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
        public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
    }
}
