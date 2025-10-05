using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class MetodoPagoService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int metodoPagoId)
    {
        return await contexto.MetodosPagos.AnyAsync(m => m.MetodoPagoId == metodoPagoId);
    }

    public async Task<bool> Insertar(MetodoPagos metodoPago)
    {
        try
        {
            contexto.MetodosPagos.Add(metodoPago);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar método de pago: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(MetodoPagos metodoPago)
    {
        try
        {
            contexto.MetodosPagos.Update(metodoPago);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar método de pago: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(MetodoPagos metodoPago)
    {
        if (!await Existe(metodoPago.MetodoPagoId))
            return await Insertar(metodoPago);
        else
            return await Modificar(metodoPago);
    }

    public async Task<bool> Eliminar(int metodoPagoId)
    {
        try
        {
            var metodo = await contexto.MetodosPagos
                .Include(m => m.Facturas)
                .FirstOrDefaultAsync(m => m.MetodoPagoId == metodoPagoId);

            if (metodo == null)
                return false;

            if (metodo.Facturas.Any())
                contexto.Facturas.RemoveRange(metodo.Facturas);

            contexto.MetodosPagos.Remove(metodo);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar método de pago: {ex.Message}");
            return false;
        }
    }

    public async Task<MetodoPagos?> Buscar(int metodoPagoId)
    {
        return await contexto.MetodosPagos
            .Include(m => m.Facturas)
            .FirstOrDefaultAsync(m => m.MetodoPagoId == metodoPagoId);
    }

    public async Task<List<MetodoPagos>> Listar(Expression<Func<MetodoPagos, bool>> criterio)
    {
        return await contexto.MetodosPagos
            .Where(criterio)
            .Include(m => m.Facturas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<MetodoPagos>> ListarMetodosPagos()
    {
        return await contexto.MetodosPagos
            .AsNoTracking()
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }
}