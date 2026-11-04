using InventarioAPI.Dto;
using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Creador")]
        public async Task<IActionResult> Create(ProductoDto dto)
        {
            var producto = await _productoRepository.CreateAsync(new Producto()
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = (double)dto.Precio,
                Stock = dto.Stock
            });
            return Ok(producto);
        }

        [HttpGet("{skip}/{take}")]
        [Authorize(Roles = "Visualizador")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var result = await _productoRepository.GetAsync(skip, take);
            return Ok(result);
        }

        [HttpGet("reporte-inversion")]
        [Authorize(Roles = "Visualizador")]
        public async Task<IActionResult> GetReporte()
        {
            var reporte = await _productoRepository.GetReporteInventarioAsync();

            var docs = (List<MongoDB.Bson.BsonDocument>)reporte;
            var json = docs.Select(d => new {
                Proveedor = d["_id"].AsString,
                TotalStock = d["TotalStock"].AsInt32,
                ValorInversion = d["ValorInversion"].AsDouble
            });

            return Ok(json);
        }
    }
}