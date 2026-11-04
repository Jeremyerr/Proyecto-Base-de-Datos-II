using InventarioAPI.Models;

namespace InventarioAPI.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task<object> GetReporteInventarioAsync();
    }
}