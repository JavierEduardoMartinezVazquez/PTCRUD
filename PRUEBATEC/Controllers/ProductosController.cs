using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRUEBATEC.Models;
using PRUEBATEC.Repositories;
using System;
using System.Linq;
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

        // LISTA DE PRODUCTOS
        public async Task<IActionResult> Index(string busqueda, string sortOrder, int pagina = 1)
        {
            try
            {
                int registrosPorPagina = 5;

                ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
                ViewData["PrecioVSortParam"] = sortOrder == "PrecioV" ? "precioV_desc" : "PrecioV";
                ViewData["PrecioCSortParam"] = sortOrder == "PrecioC" ? "precioC_desc" : "PrecioC";

                var productos = await _repository.ObtenerPaginadoAsync(busqueda, pagina, registrosPorPagina);

                // Ordenamiento
                productos = sortOrder switch
                {
                    "nombre_desc" => productos.OrderByDescending(p => p.NombreP),
                    "PrecioV" => productos.OrderBy(p => p.PrecioV),
                    "precioV_desc" => productos.OrderByDescending(p => p.PrecioV),
                    "PrecioC" => productos.OrderBy(p => p.PrecioC),
                    "precioC_desc" => productos.OrderByDescending(p => p.PrecioC),
                    _ => productos.OrderBy(p => p.NombreP),
                };

                var totalRegistros = productos.Count();
                var productosPaginados = productos
                    .Skip((pagina - 1) * registrosPorPagina)
                    .Take(registrosPorPagina)
                    .ToList();

                ViewBag.Busqueda = busqueda;
                ViewBag.PaginaActual = pagina;
                ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
                ViewBag.SortOrder = sortOrder;

                return View(productosPaginados);
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = $"Error al cargar los productos: {ex.Message}";
                TempData["TipoAlerta"] = "error";
                return View(Enumerable.Empty<ProductoModel>());
            }
        }

        //CREAR PRODUCTO
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.CrearAsync(producto);
                    TempData["Alerta"] = "Producto creado exitosamente";
                    TempData["TipoAlerta"] = "success";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Alerta"] = "Datos inválidos. Por favor revisa los campos.";
                TempData["TipoAlerta"] = "warning";
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = $"Error al crear el producto: {ex.Message}";
                TempData["TipoAlerta"] = "error";
                return View(producto);
            }
        }

        //EDITAR PRODUCTO
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                TempData["Alerta"] = "El producto no existe.";
                TempData["TipoAlerta"] = "error";
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoModel producto)
        {
            if (id != producto.Id)
            {
                TempData["Alerta"] = "El identificador del producto no coincide.";
                TempData["TipoAlerta"] = "error";
                return BadRequest();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.ActualizarAsync(producto);
                    TempData["Alerta"] = "Producto actualizado correctamente.";
                    TempData["TipoAlerta"] = "success";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Alerta"] = "Datos inválidos. No se pudo actualizar.";
                TempData["TipoAlerta"] = "warning";
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = $"Error al actualizar el producto: {ex.Message}";
                TempData["TipoAlerta"] = "error";
                return View(producto);
            }
        }

        // CONFIRMAR ELIMINACIÓN
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                TempData["Alerta"] = "El producto no existe.";
                TempData["TipoAlerta"] = "error";
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // ELIMINAR PRODUCTO
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.EliminarAsync(id);
                TempData["Alerta"] = "Producto eliminado correctamente.";
                TempData["TipoAlerta"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = $"Error al eliminar el producto: {ex.Message}";
                TempData["TipoAlerta"] = "error";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
