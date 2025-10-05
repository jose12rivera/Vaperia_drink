using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class InventarioService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int inventarioId)
    {
        return await contexto.Inventarios.AnyAsync(i => i.InventarioId == inventarioId);
    }

    public async Task<bool> Insertar(Inventarios inventario)
    {
        try
        {
            contexto.Inventarios.Add(inventario);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar inventario: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Inventarios inventario)
    {
        try
        {
            contexto.Inventarios.Update(inventario);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar inventario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Inventarios inventario)
    {
        if (!await Existe(inventario.InventarioId))
            return await Insertar(inventario);
        else
            return await Modificar(inventario);
    }

    public async Task<bool> Eliminar(int inventarioId)
    {
        try
        {
            var inventario = await contexto.Inventarios
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(i => i.InventarioId == inventarioId);

            if (inventario == null)
                return false;

            contexto.Inventarios.Remove(inventario);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar inventario: {ex.Message}");
            return false;
        }
    }

    public async Task<Inventarios?> Buscar(int inventarioId)
    {
        return await contexto.Inventarios
            .Include(i => i.Producto)
            .FirstOrDefaultAsync(i => i.InventarioId == inventarioId);
    }

    public async Task<List<Inventarios>> Listar(Expression<Func<Inventarios, bool>> criterio)
    {
        return await contexto.Inventarios
            .Where(criterio)
            .Include(i => i.Producto)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Inventarios>> ListarInventarios()
    {
        return await contexto.Inventarios
            .Include(i => i.Producto)
            .AsNoTracking()
            .OrderByDescending(i => i.FechaMovimiento)
            .ToListAsync();
    }
}