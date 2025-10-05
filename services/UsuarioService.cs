using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class UsuarioService
{
    private readonly ApplicationDbContext _contexto;

    public UsuarioService(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    // Verificar si existe un usuario por Id
    public async Task<bool> Existe(int usuarioId)
    {
        return await _contexto.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
    }

    // Crear nuevo usuario
    public async Task<bool> Crear(Usuarios usuario)
    {
        try
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear usuario: {ex.Message}");
            return false;
        }
    }

    // Modificar usuario existente
    public async Task<bool> Modificar(Usuarios usuario)
    {
        try
        {
            _contexto.Usuarios.Update(usuario);
            return await _contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar usuario: {ex.Message}");
            return false;
        }
    }

    // Guardar usuario (decide si insertar o modificar)
    public async Task<bool> Guardar(Usuarios usuario)
    {
        if (!await Existe(usuario.UsuarioId))
            return await Crear(usuario);
        else
            return await Modificar(usuario);
    }

    // Eliminar usuario por Id
    public async Task<bool> Eliminar(int usuarioId)
    {
        try
        {
            var usuario = await _contexto.Usuarios
                .Include(u => u.Ventas)
                .Include(u => u.Facturas)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            if (usuario == null)
                return false;

            if (usuario.Ventas.Any())
                _contexto.RemoveRange(usuario.Ventas);

            if (usuario.Facturas.Any())
                _contexto.RemoveRange(usuario.Facturas);

            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            return false;
        }
    }

    // Buscar usuario por Id
    public async Task<Usuarios?> Buscar(int usuarioId)
    {
        return await _contexto.Usuarios
            .Include(u => u.Ventas)
            .Include(u => u.Facturas)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
    }

    // Listar usuarios con filtro
    public async Task<List<Usuarios>> Listar(Expression<Func<Usuarios, bool>> criterio)
    {
        return await _contexto.Usuarios
            .Where(criterio)
            .Include(u => u.Ventas)
            .Include(u => u.Facturas)
            .AsNoTracking()
            .ToListAsync();
    }

    // Listar todos los usuarios
    public async Task<List<Usuarios>> ListarUsuarios()
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .OrderByDescending(u => u.UsuarioId)
            .ToListAsync();
    }
}
