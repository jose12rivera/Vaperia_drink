using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class FacturaService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int facturaId)
    {
        return await contexto.Facturas.AnyAsync(f => f.FacturaId == facturaId);
    }

    public async Task<bool> Insertar(Facturas factura)
    {
        try
        {
            if (factura.VentaId != 0)
                factura.Venta = await contexto.Ventas.FindAsync(factura.VentaId);

            if (factura.ClienteId != 0)
                factura.Cliente = await contexto.Clientes.FindAsync(factura.ClienteId);

            if (factura.UsuarioId != 0)
                factura.Usuario = await contexto.Usuarios.FindAsync(factura.UsuarioId);

            if (factura.MetodoPagoId != 0)
                factura.MetodoPago = await contexto.MetodosPagos.FindAsync(factura.MetodoPagoId);

            contexto.Facturas.Add(factura);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar factura: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Facturas factura)
    {
        try
        {
            contexto.Facturas.Update(factura);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar factura: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Facturas factura)
    {
        if (!await Existe(factura.FacturaId))
            return await Insertar(factura);
        else
            return await Modificar(factura);
    }

    public async Task<bool> Eliminar(int facturaId)
    {
        try
        {
            var factura = await contexto.Facturas
                .FirstOrDefaultAsync(f => f.FacturaId == facturaId);

            if (factura == null)
                return false;

            contexto.Facturas.Remove(factura);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar factura: {ex.Message}");
            return false;
        }
    }

    public async Task<Facturas?> Buscar(int facturaId)
    {
        return await contexto.Facturas
            .Include(f => f.Venta)
            .Include(f => f.Cliente)
            .Include(f => f.Usuario)
            .Include(f => f.MetodoPago)
            .FirstOrDefaultAsync(f => f.FacturaId == facturaId);
    }

    public async Task<List<Facturas>> Listar(Expression<Func<Facturas, bool>> criterio)
    {
        return await contexto.Facturas
            .Where(criterio)
            .Include(f => f.Venta)
            .Include(f => f.Cliente)
            .Include(f => f.Usuario)
            .Include(f => f.MetodoPago)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Facturas>> ListarFacturas()
    {
        return await contexto.Facturas
            .Include(f => f.Venta)
            .Include(f => f.Cliente)
            .Include(f => f.Usuario)
            .Include(f => f.MetodoPago)
            .AsNoTracking()
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<List<Facturas>> ObtenerPorCliente(int clienteId)
    {
        return await contexto.Facturas
            .Where(f => f.ClienteId == clienteId)
            .Include(f => f.Venta)
            .Include(f => f.MetodoPago)
            .AsNoTracking()
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<List<Facturas>> ObtenerPorUsuario(int usuarioId)
    {
        return await contexto.Facturas
            .Where(f => f.UsuarioId == usuarioId)
            .Include(f => f.Cliente)
            .Include(f => f.MetodoPago)
            .AsNoTracking()
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<List<Facturas>> ObtenerPorRangoFecha(DateTime desde, DateTime hasta)
    {
        return await contexto.Facturas
            .Where(f => f.FechaEmision >= desde && f.FechaEmision <= hasta)
            .Include(f => f.Cliente)
            .Include(f => f.MetodoPago)
            .AsNoTracking()
            .OrderBy(f => f.FechaEmision)
            .ToListAsync();
    }

    public async Task<decimal> ObtenerTotalFacturado()
    {
        return await contexto.Facturas.SumAsync(f => f.Total);
    }

    public async Task<string?> ObtenerUltimoNumeroFactura()
    {
        return await contexto.Facturas
            .OrderByDescending(f => f.FacturaId)
            .Select(f => f.NumeroFactura)
            .FirstOrDefaultAsync();
    }
}