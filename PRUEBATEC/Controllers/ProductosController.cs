using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRUEBATEC.Models;
using PRUEBATEC.Repositories;
using System.Threading.Tasks;

namespace PRUEBATEC.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _repository;

        public ProductosController(IProductoRepository repository)
        {
            _repository = repository;
        }

        // GET: Vista de los productos
        public async Task<IActionResult> Index(string busqueda, string sortOrder, int pagina = 1)
        {
            int registrosPorPagina = 5;

            // Parámetros de ordenamiento para la vista
            ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["PrecioVSortParam"] = sortOrder == "PrecioV" ? "precioV_desc" : "PrecioV";
            ViewData["PrecioCSortParam"] = sortOrder == "PrecioC" ? "precioC_desc" : "PrecioC";

            // Obtener todos los productos filtrados por búsqueda
            var productos = await _repository.ObtenerPaginadoAsync(busqueda, pagina, registrosPorPagina); ;

            // Aplicar ordenamiento en memoria (o en el repo si prefieres)
            switch (sortOrder)
            {
                case "nombre_desc":
                    productos = productos.OrderByDescending(p => p.NombreP);
                    break;
                case "PrecioV":
                    productos = productos.OrderBy(p => p.PrecioV);
                    break;
                case "precioV_desc":
                    productos = productos.OrderByDescending(p => p.PrecioV);
                    break;
                case "PrecioC":
                    productos = productos.OrderBy(p => p.PrecioC);
                    break;
                case "precioC_desc":
                    productos = productos.OrderByDescending(p => p.PrecioC);
                    break;
                default:
                    productos = productos.OrderBy(p => p.NombreP);
                    break;
            }

            // Paginación 
            var totalRegistros = productos.Count();
            var productosPaginados = productos
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToList();

            // Pasar datos a la vista
            ViewBag.Busqueda = busqueda;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.SortOrder = sortOrder;

            return View(productosPaginados);
        }


        // Vista de Crer producto
        public IActionResult Create()
        {
            return View();
        }

        // POST: Para modificación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel producto)
        {
            if (ModelState.IsValid)
            {
                await _repository.CrearAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Obtener por Id para actualizar información
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // POST: Enviar por el Id para actualizar información
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoModel producto)
        {
            if (id != producto.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _repository.ActualizarAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Eliminación por Id
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // POST: Eliminación por Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
