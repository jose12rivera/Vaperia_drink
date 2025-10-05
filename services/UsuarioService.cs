using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class UsuarioService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int usuarioId)
    {
        return await contexto.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
    }

    public async Task<bool> Insertar(Usuarios usuario)
    {
        try
        {
            contexto.Usuarios.Add(usuario);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar usuario: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Usuarios usuario)
    {
        try
        {
            contexto.Usuarios.Update(usuario);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Usuarios usuario)
    {
        if (!await Existe(usuario.UsuarioId))
            return await Insertar(usuario);
        else
            return await Modificar(usuario);
    }

    public async Task<bool> Eliminar(int usuarioId)
    {
        try
        {
            var usuario = await contexto.Usuarios
                .Include(u => u.Ventas)
                .Include(u => u.Facturas)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            if (usuario == null)
                return false;

            if (usuario.Ventas.Any())
                contexto.RemoveRange(usuario.Ventas);

            if (usuario.Facturas.Any())
                contexto.RemoveRange(usuario.Facturas);

            contexto.Usuarios.Remove(usuario);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<Usuarios?> Buscar(int usuarioId)
    {
        return await contexto.Usuarios
            .Include(u => u.Ventas)
            .Include(u => u.Facturas)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
    }

    public async Task<List<Usuarios>> Listar(Expression<Func<Usuarios, bool>> criterio)
    {
        return await contexto.Usuarios
            .Where(criterio)
            .Include(u => u.Ventas)
            .Include(u => u.Facturas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Usuarios>> ListarUsuarios()
    {
        return await contexto.Usuarios
            .AsNoTracking()
            .OrderByDescending(u => u.UsuarioId)
            .ToListAsync();
    }
}