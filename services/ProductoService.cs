using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class ProductoService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int productoId)
    {
        return await contexto.Productos.AnyAsync(p => p.ProductoId == productoId);
    }

    public async Task<bool> Insertar(Productos producto)
    {
        try
        {
            contexto.Productos.Add(producto);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar producto: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Productos producto)
    {
        try
        {
            contexto.Productos.Update(producto);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar producto: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Productos producto)
    {
        if (!await Existe(producto.ProductoId))
            return await Insertar(producto);
        else
            return await Modificar(producto);
    }

    public async Task<bool> Eliminar(int productoId)
    {
        try
        {
            var producto = await contexto.Productos
                .Include(p => p.DetallesVenta)
                .Include(p => p.MovimientosInventario)
                .FirstOrDefaultAsync(p => p.ProductoId == productoId);

            if (producto == null)
                return false;

            if (producto.DetallesVenta.Any())
                contexto.DetalleVentas.RemoveRange(producto.DetallesVenta);

            if (producto.MovimientosInventario.Any())
                contexto.Inventarios.RemoveRange(producto.MovimientosInventario);

            contexto.Productos.Remove(producto);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto: {ex.Message}");
            return false;
        }
    }

    public async Task<Productos?> Buscar(int productoId)
    {
        return await contexto.Productos
            .Include(p => p.DetallesVenta)
            .Include(p => p.MovimientosInventario)
            .FirstOrDefaultAsync(p => p.ProductoId == productoId);
    }

    public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
    {
        return await contexto.Productos
            .Where(criterio)
            .Include(p => p.DetallesVenta)
            .Include(p => p.MovimientosInventario)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Productos>> ListarProductos()
    {
        return await contexto.Productos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ActualizarStock(int productoId, int cantidad, bool esEntrada)
    {
        try
        {
            var producto = await contexto.Productos.FindAsync(productoId);
            if (producto == null)
                return false;

            if (esEntrada)
                producto.Stock += cantidad;
            else
                producto.Stock -= cantidad;

            contexto.Productos.Update(producto);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar stock: {ex.Message}");
            return false;
        }
    }
}