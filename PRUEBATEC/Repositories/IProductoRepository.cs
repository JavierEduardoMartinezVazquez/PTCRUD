using PRUEBATEC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRUEBATEC.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<ProductoModel>> ObtenerPaginadoAsync(string busqueda, int pagina, int registrosPorPagina);
        Task<int> ContarTotalAsync(string busqueda);
        Task<ProductoModel> ObtenerPorIdAsync(int id);
        Task CrearAsync(ProductoModel producto);
        Task ActualizarAsync(ProductoModel producto);
        Task EliminarAsync(int id);
    }
}
