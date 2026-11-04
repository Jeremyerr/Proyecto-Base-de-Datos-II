using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using MongoDB.Driver;

namespace InventarioAPI.Repositorio
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(IMongoDatabase database) : base(database, "productos")
        {
        }

        public async Task<object> GetReporteInventarioAsync()
        {
            var pipeline = new MongoDB.Bson.BsonDocument[]
            {
                new MongoDB.Bson.BsonDocument("$group", new MongoDB.Bson.BsonDocument
                {
                    { "_id", "$Proveedor.Nombre" },
                    { "TotalStock", new MongoDB.Bson.BsonDocument("$sum", "$Stock") },
                    { "ValorInversion", new MongoDB.Bson.BsonDocument("$sum",
                        new MongoDB.Bson.BsonDocument("$multiply", new MongoDB.Bson.BsonArray { "$Precio", "$Stock" })) }
                }),
                new MongoDB.Bson.BsonDocument("$sort", new MongoDB.Bson.BsonDocument("ValorInversion", -1))
            };

            return await _collection.Aggregate<MongoDB.Bson.BsonDocument>(pipeline).ToListAsync();
        }
    }
}