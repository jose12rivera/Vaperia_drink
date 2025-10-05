using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class EstadisticaVentaService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int estadisticaId)
    {
        return await contexto.EstadisticaVentas.AnyAsync(e => e.EstadisticaVentaId == estadisticaId);
    }

    public async Task<bool> Insertar(EstadisticaVentas estadistica)
    {
        try
        {
            contexto.EstadisticaVentas.Add(estadistica);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar estadística: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(EstadisticaVentas estadistica)
    {
        try
        {
            contexto.EstadisticaVentas.Update(estadistica);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar estadística: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(EstadisticaVentas estadistica)
    {
        if (!await Existe(estadistica.EstadisticaVentaId))
            return await Insertar(estadistica);
        else
            return await Modificar(estadistica);
    }

    public async Task<bool> Eliminar(int estadisticaId)
    {
        try
        {
            var estadistica = await contexto.EstadisticaVentas
                .FirstOrDefaultAsync(e => e.EstadisticaVentaId == estadisticaId);

            if (estadistica == null)
                return false;

            contexto.EstadisticaVentas.Remove(estadistica);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar estadística: {ex.Message}");
            return false;
        }
    }

    public async Task<EstadisticaVentas?> Buscar(int estadisticaId)
    {
        return await contexto.EstadisticaVentas
            .FirstOrDefaultAsync(e => e.EstadisticaVentaId == estadisticaId);
    }

    public async Task<List<EstadisticaVentas>> Listar(Expression<Func<EstadisticaVentas, bool>> criterio)
    {
        return await contexto.EstadisticaVentas
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<EstadisticaVentas>> ListarEstadisticas()
    {
        return await contexto.EstadisticaVentas
            .AsNoTracking()
            .OrderByDescending(e => e.Fecha)
            .ToListAsync();
    }

    public async Task<List<EstadisticaVentas>> ObtenerPorRangoFecha(DateTime desde, DateTime hasta)
    {
        return await contexto.EstadisticaVentas
            .Where(e => e.Fecha >= desde && e.Fecha <= hasta)
            .OrderBy(e => e.Fecha)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(int totalVentas, decimal totalVendido, int productosVendidos)> ObtenerTotales()
    {
        var totalVentas = await contexto.EstadisticaVentas.SumAsync(e => e.CantidadVentas);
        var totalVendido = await contexto.EstadisticaVentas.SumAsync(e => e.TotalVendido);
        var productosVendidos = await contexto.EstadisticaVentas.SumAsync(e => e.ProductosVendidos);

        return (totalVentas, totalVendido, productosVendidos);
    }

    public async Task<EstadisticaVentas?> ObtenerEstadisticaDelDia()
    {
        var hoy = DateTime.Today;
        return await contexto.EstadisticaVentas
            .FirstOrDefaultAsync(e => e.Fecha.Date == hoy);
    }
}