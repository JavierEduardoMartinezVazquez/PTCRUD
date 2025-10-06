using Microsoft.EntityFrameworkCore;
using PRUEBATEC.Data;
using PRUEBATEC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBATEC.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoModel>> ObtenerPaginadoAsync(string busqueda, int pagina, int registrosPorPagina)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
                query = query.Where(p => p.NombreP.Contains(busqueda));

            return await query
                .OrderBy(p => p.Id)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();
        }

        public async Task<int> ContarTotalAsync(string busqueda)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
                query = query.Where(p => p.NombreP.Contains(busqueda));

            return await query.CountAsync();
        }

        public async Task<ProductoModel> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task CrearAsync(ProductoModel producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(ProductoModel producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        // Nuevo método: traer todos los productos
        public async Task<IEnumerable<ProductoModel>> ObtenerTodosAsync()
        {
            return await _context.Productos
                                 .OrderBy(p => p.NombreP)
                                 .ToListAsync();
        }
    }
}
