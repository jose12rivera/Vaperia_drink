using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaperia_drink.Data;
using Vaperia_drink.Models;

namespace Vaperia_drink.Services;

public class ClienteService(ApplicationDbContext contexto)
{
    public async Task<bool> Existe(int clienteId)
    {
        return await contexto.Clientes.AnyAsync(c => c.ClienteId == clienteId);
    }

    public async Task<bool> Insertar(Clientes cliente)
    {
        try
        {
            contexto.Clientes.Add(cliente);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar cliente: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> Modificar(Clientes cliente)
    {
        try
        {
            contexto.Clientes.Update(cliente);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al modificar cliente: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Guardar(Clientes cliente)
    {
        if (!await Existe(cliente.ClienteId))
            return await Insertar(cliente);
        else
            return await Modificar(cliente);
    }

    public async Task<bool> Eliminar(int clienteId)
    {
        try
        {
            var cliente = await contexto.Clientes
                .Include(c => c.Ventas)
                .Include(c => c.Facturas)
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId);

            if (cliente == null)
                return false;

            if (cliente.Ventas.Any())
                contexto.RemoveRange(cliente.Ventas);

            if (cliente.Facturas.Any())
                contexto.RemoveRange(cliente.Facturas);

            contexto.Clientes.Remove(cliente);
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
            return false;
        }
    }

    public async Task<Clientes?> Buscar(int clienteId)
    {
        return await contexto.Clientes
            .Include(c => c.Ventas)
            .Include(c => c.Facturas)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        return await contexto.Clientes
            .Where(criterio)
            .Include(c => c.Ventas)
            .Include(c => c.Facturas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Clientes>> ListarClientes()
    {
        return await contexto.Clientes
            .AsNoTracking()
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync();
    }
}