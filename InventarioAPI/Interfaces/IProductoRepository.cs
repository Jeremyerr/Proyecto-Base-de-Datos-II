using InventarioAPI.Dto;
using InventarioAPI.Models;

namespace InventarioAPI.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task<object> GetReporteInventarioAsync();
        Task<Producto?> ActualizarAsync(string id, Producto datos);
        Task<PaginacionResult<Producto>> BuscarAsync(string? categoria, double? precioMin, double? precioMax, string? proveedor, string? etiqueta, bool soloConStock, string ordenarPor, bool descendente, int pagina, int tamano);
    }
}