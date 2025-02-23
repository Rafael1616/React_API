using API.Models;
using APIREST.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class Controlador : ControllerBase
    {
        private ProductoDAO _dao;
        public Controlador(ProductoDAO dao)
        {
            _dao = dao;
        }

        [HttpGet("Listarproducto")]
        public async Task<ActionResult<List<Producto>>> ObtenerListado()
        {
            var productos = await _dao.SelectAllAsync();
            // Si no hay productos, retornamos una lista vacía  
            return Ok(productos);
        }

        [HttpGet("Obtenerporid/{id}")]
        public async Task<ActionResult<Producto>> ObtenerProducto(int id)
        {
            var producto = await _dao.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound($"No se encontró el producto con ID {id}");
            }
            return Ok(producto);
        }


        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool eliminado = await _dao.DeleteProductoAsync(id);
            if (eliminado)
            {
                return Ok(new { mensaje = "Producto eliminado con éxito" });
            }
            return NotFound(new { mensaje = "No se encontró el producto con el ID especificado" });
        }


        [HttpPost("Insertar")]
        public async Task<ActionResult<Producto>> InsertarProducto([FromBody] Producto producto)
        {
            if (producto == null || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                return BadRequest("Datos de producto inválidos.");
            }

            var resultado = await _dao.InsertProductoAsync(producto);
            if (resultado == null)
            {
                return StatusCode(500, "Error al insertar el producto.");
            }

            return CreatedAtAction(nameof(ObtenerProducto), new { id = resultado.Id }, resultado);
        }


        [HttpPut("Actualizar")]
        public async Task<ActionResult<Producto>> ActualizarProducto([FromBody] Producto producto)
        {
            var resultado = await _dao.UpdateProductoAsync(producto);

            if (!resultado)
            {
                return StatusCode(500, "Error al actualizar el producto.");
            }

            return Ok(producto);
        }

    }
}
