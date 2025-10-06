using Microsoft.AspNetCore.Mvc;
using PRUEBATEC.Models;
using PRUEBATEC.Repositories;
using System.Threading.Tasks;

namespace PRUEBATEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosApiController : ControllerBase
    {
        private readonly IProductoRepository _repository;

        public ProductosApiController(IProductoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string busqueda = null, int pagina = 1, int registros = 5)
        {
            var productos = await _repository.ObtenerPaginadoAsync(busqueda, pagina, registros);
            var total = await _repository.ContarTotalAsync(busqueda);
            return Ok(new { total, productos });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoModel producto)
        {
            await _repository.CrearAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoModel producto)
        {
            if (id != producto.Id) return BadRequest();
            await _repository.ActualizarAsync(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.EliminarAsync(id);
            return NoContent();
        }
    }
}
