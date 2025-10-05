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
}