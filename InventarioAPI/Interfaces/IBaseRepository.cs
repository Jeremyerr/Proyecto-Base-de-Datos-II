using InventarioAPI.Dto;
using InventarioAPI.Models;

namespace InventarioAPI.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<PaginacionResult<TEntity>> GetAsync(int skip, int take);
    }
}