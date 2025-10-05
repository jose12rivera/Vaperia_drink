using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class DetalleVentaService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int detalleVentaId)
    {
        return await contexto.DetalleVentas.AnyAsync(d => d.DetalleVentaId == detalleVentaId);
    }

    public async Task<bool> Insertar(DetalleVentas detalle)
    {
        try
        {
            if (detalle.ProductoId != 0)
                detalle.Producto = await contexto.Productos.FindAsync(detalle.ProductoId);

            if (detalle.UsuarioId != 0)
                detalle.Usuario = await contexto.Usuarios.FindAsync(detalle.UsuarioId);

            if (detalle.VentaId != 0)
                detalle.Venta = await contexto.Ventas.FindAsync(detalle.VentaId);

            contexto.DetalleVentas.Add(detalle);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar detalle de venta: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(DetalleVentas detalle)
    {
        try
        {
            contexto.DetalleVentas.Update(detalle);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar detalle de venta: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(DetalleVentas detalle)
    {
        if (!await Existe(detalle.DetalleVentaId))
            return await Insertar(detalle);
        else
            return await Modificar(detalle);
    }

    public async Task<bool> Eliminar(int detalleVentaId)
    {
        try
        {
            var detalle = await contexto.DetalleVentas
                .FirstOrDefaultAsync(d => d.DetalleVentaId == detalleVentaId);

            if (detalle == null)
                return false;

            contexto.DetalleVentas.Remove(detalle);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar detalle de venta: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarPorVenta(int ventaId)
    {
        try
        {
            var detalles = await contexto.DetalleVentas
                .Where(d => d.VentaId == ventaId)
                .ToListAsync();

            if (detalles.Any())
            {
                contexto.DetalleVentas.RemoveRange(detalles);
                await contexto.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar detalles de venta: {ex.Message}");
            return false;
        }
    }

    public async Task<DetalleVentas?> Buscar(int detalleVentaId)
    {
        return await contexto.DetalleVentas
            .Include(d => d.Producto)
            .Include(d => d.Venta)
            .Include(d => d.Usuario)
            .FirstOrDefaultAsync(d => d.DetalleVentaId == detalleVentaId);
    }

    public async Task<List<DetalleVentas>> Listar(Expression<Func<DetalleVentas, bool>> criterio)
    {
        return await contexto.DetalleVentas
            .Where(criterio)
            .Include(d => d.Producto)
            .Include(d => d.Usuario)
            .Include(d => d.Venta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<DetalleVentas>> ListarDetalles()
    {
        return await contexto.DetalleVentas
            .Include(d => d.Producto)
            .Include(d => d.Usuario)
            .Include(d => d.Venta)
            .AsNoTracking()
            .OrderByDescending(d => d.DetalleVentaId)
            .ToListAsync();
    }

    public async Task<List<DetalleVentas>> ObtenerPorProducto(int productoId)
    {
        return await contexto.DetalleVentas
            .Include(d => d.Producto)
            .Include(d => d.Venta)
            .Where(d => d.ProductoId == productoId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<DetalleVentas>> ObtenerPorUsuario(int usuarioId)
    {
        return await contexto.DetalleVentas
            .Include(d => d.Producto)
            .Include(d => d.Venta)
            .Include(d => d.Usuario)
            .Where(d => d.UsuarioId == usuarioId)
            .AsNoTracking()
            .ToListAsync();
    }
}