using InventarioAPI.Dto;
using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using MongoDB.Driver;

namespace InventarioAPI.Repositorio
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public BaseRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<PaginacionResult<TEntity>> GetAsync(int skip, int take)
        {
            var total = await _collection.CountDocumentsAsync(_ => true);
            var result = await _collection.Find(_ => true).Skip(skip).Limit(take).ToListAsync();
            return new PaginacionResult<TEntity> { Result = result, Total = (int)total };
        }
    }
}