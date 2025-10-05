using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class VentasService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int ventaId)
    {
        return await contexto.Ventas.AnyAsync(v => v.VentaId == ventaId);
    }

    public async Task<bool> Insertar(Ventas venta)
    {
        try
        {
            venta.Cliente = await contexto.Clientes.FindAsync(venta.ClienteId);
            venta.Usuario = await contexto.Usuarios.FindAsync(venta.UsuarioId);

            contexto.Ventas.Add(venta);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar venta: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Ventas venta)
    {
        try
        {
            contexto.Ventas.Update(venta);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar venta: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Ventas venta)
    {
        if (!await Existe(venta.VentaId))
            return await Insertar(venta);
        else
            return await Modificar(venta);
    }

    public async Task<bool> Eliminar(int ventaId)
    {
        try
        {
            var venta = await contexto.Ventas
                .Include(v => v.Detalles)
                .Include(v => v.Facturas)
                .FirstOrDefaultAsync(v => v.VentaId == ventaId);

            if (venta == null)
                return false;

            if (venta.Detalles.Any())
                contexto.DetalleVentas.RemoveRange(venta.Detalles);

            if (venta.Facturas.Any())
                contexto.Facturas.RemoveRange(venta.Facturas);

            contexto.Ventas.Remove(venta);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar venta: {ex.Message}");
            return false;
        }
    }

    public async Task<Ventas?> Buscar(int ventaId)
    {
        return await contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
            .Include(v => v.Facturas)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId);
    }

    public async Task<List<Ventas>> Listar(Expression<Func<Ventas, bool>> criterio)
    {
        return await contexto.Ventas
            .Where(criterio)
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
            .Include(v => v.Facturas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Ventas>> ListarVentas()
    {
        return await contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
            .Include(v => v.Facturas)
            .AsNoTracking()
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    public async Task<List<Ventas>> ListarVentasPorCliente(int clienteId)
    {
        return await contexto.Ventas
            .Where(v => v.ClienteId == clienteId)
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
            .Include(v => v.Facturas)
            .AsNoTracking()
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    public async Task<List<Ventas>> ListarVentasPorUsuario(int usuarioId)
    {
        return await contexto.Ventas
            .Where(v => v.UsuarioId == usuarioId)
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
            .Include(v => v.Facturas)
            .AsNoTracking()
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    // MÉTODO NUEVO: Guardar venta completa con detalles y actualización de inventario
    public async Task<(bool Success, string Message, int? VentaId)> GuardarVentaCompleta(Ventas venta, List<DetalleVentas> detalles)
    {
        using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            // 1. Validar que haya stock suficiente para todos los productos
            foreach (var detalle in detalles)
            {
                var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
                if (producto == null)
                {
                    return (false, $"Producto con ID {detalle.ProductoId} no encontrado", null);
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    return (false, $"Stock insuficiente para {producto.Nombre}. Disponible: {producto.Stock}, Solicitado: {detalle.Cantidad}", null);
                }
            }

            // 2. Cargar las entidades relacionadas
            venta.Cliente = await contexto.Clientes.FindAsync(venta.ClienteId);
            venta.Usuario = await contexto.Usuarios.FindAsync(venta.UsuarioId);

            if (venta.Cliente == null)
            {
                return (false, "Cliente no encontrado", null);
            }

            if (venta.Usuario == null)
            {
                return (false, "Usuario no encontrado", null);
            }

            // 3. Guardar la venta
            contexto.Ventas.Add(venta);
            await contexto.SaveChangesAsync();

            // 4. Guardar los detalles y actualizar inventario
            foreach (var detalle in detalles)
            {
                // Asignar el VentaId generado
                detalle.VentaId = venta.VentaId;

                // Cargar el producto y usuario
                detalle.Producto = await contexto.Productos.FindAsync(detalle.ProductoId);
                detalle.Usuario = await contexto.Usuarios.FindAsync(detalle.UsuarioId);

                // Agregar el detalle
                contexto.DetalleVentas.Add(detalle);

                // 5. Actualizar el stock del producto
                var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock -= detalle.Cantidad;
                    contexto.Productos.Update(producto);

                    // 6. Registrar el movimiento de inventario (salida)
                    var movimientoInventario = new Inventarios
                    {
                        ProductoId = detalle.ProductoId,
                        Producto = producto,
                        FechaMovimiento = DateTime.Now,
                        Cantidad = detalle.Cantidad,
                        TipoMovimiento = "Salida"
                    };
                    contexto.Inventarios.Add(movimientoInventario);
                }
            }

            // 7. Guardar todos los cambios
            await contexto.SaveChangesAsync();

            // 8. Confirmar la transacción
            await transaction.CommitAsync();

            return (true, "Venta completada exitosamente", venta.VentaId);
        }
        catch (Exception ex)
        {
            // Revertir la transacción en caso de error
            await transaction.RollbackAsync();
            Console.WriteLine($"Error al guardar venta completa: {ex.Message}");
            return (false, $"Error al procesar la venta: {ex.Message}", null);
        }
    }

    // MÉTODO NUEVO: Anular una venta (restaura el stock)
    public async Task<(bool Success, string Message)> AnularVenta(int ventaId, int usuarioId)
    {
        using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var venta = await contexto.Ventas
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.VentaId == ventaId);

            if (venta == null)
            {
                return (false, "Venta no encontrada");
            }

            // Restaurar el stock de cada producto
            foreach (var detalle in venta.Detalles)
            {
                var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock += detalle.Cantidad;
                    contexto.Productos.Update(producto);

                    // Registrar el movimiento de inventario (entrada por devolución)
                    var movimientoInventario = new Inventarios
                    {
                        ProductoId = detalle.ProductoId,
                        Producto = producto,
                        FechaMovimiento = DateTime.Now,
                        Cantidad = detalle.Cantidad,
                        TipoMovimiento = "Entrada"
                    };
                    contexto.Inventarios.Add(movimientoInventario);
                }
            }

            // Eliminar los detalles
            contexto.DetalleVentas.RemoveRange(venta.Detalles);

            // Eliminar facturas asociadas si existen
            if (venta.Facturas?.Any() == true)
            {
                contexto.Facturas.RemoveRange(venta.Facturas);
            }

            // Eliminar la venta
            contexto.Ventas.Remove(venta);

            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Venta anulada exitosamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error al anular venta: {ex.Message}");
            return (false, $"Error al anular la venta: {ex.Message}");
        }
    }

    // MÉTODO NUEVO: Obtener reporte de ventas por rango de fechas
    public async Task<List<Ventas>> ObtenerVentasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
    {
        return await contexto.Ventas
            .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .AsNoTracking()
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    // MÉTODO NUEVO: Obtener estadísticas de ventas
    public async Task<VentasEstadisticas> ObtenerEstadisticas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        var query = contexto.Ventas.AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(v => v.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(v => v.Fecha <= fechaFin.Value);

        var ventas = await query
            .Include(v => v.Detalles)
            .AsNoTracking()
            .ToListAsync();

        return new VentasEstadisticas
        {
            TotalVentas = ventas.Count,
            MontoTotal = ventas.Sum(v => v.Total),
            PromedioVenta = ventas.Any() ? ventas.Average(v => v.Total) : 0,
            ProductosVendidos = ventas.SelectMany(v => v.Detalles).Sum(d => d.Cantidad)
        };
    }
}

// Clase auxiliar para estadísticas
public class VentasEstadisticas
{
    public int TotalVentas { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal PromedioVenta { get; set; }
    public int ProductosVendidos { get; set; }
}