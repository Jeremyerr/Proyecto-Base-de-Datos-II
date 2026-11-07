using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using MongoDB.Driver;
using InventarioAPI.Dto;

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

        public async Task<Producto?> ActualizarAsync(string id, Producto datos)
        {
            if (!MongoDB.Bson.ObjectId.TryParse(id, out _)) return null;

            var update = Builders<Producto>.Update
                .Set(p => p.Nombre, datos.Nombre)
                .Set(p => p.Descripcion, datos.Descripcion)
                .Set(p => p.Precio, datos.Precio)
                .Set(p => p.Stock, datos.Stock)
                .Set(p => p.Categoria, datos.Categoria)
                .Set(p => p.Etiquetas, datos.Etiquetas)
                .Set(p => p.Proveedor, datos.Proveedor);

            return await _collection.FindOneAndUpdateAsync(
                p => p.Id == id,
                update,
                new FindOneAndUpdateOptions<Producto> { ReturnDocument = ReturnDocument.After });
        }

        public async Task<PaginacionResult<Producto>> BuscarAsync(
            string? categoria, double? precioMin, double? precioMax, string? proveedor,
            string? etiqueta, bool soloConStock, string ordenarPor, bool descendente,
            int pagina, int tamano)
        {
            var fb = Builders<Producto>.Filter;
            var filtros = new List<FilterDefinition<Producto>>();

            if (!string.IsNullOrWhiteSpace(categoria))
                filtros.Add(fb.Eq(p => p.Categoria, categoria));
            if (precioMin.HasValue)
                filtros.Add(fb.Gte(p => p.Precio, precioMin.Value));
            if (precioMax.HasValue)
                filtros.Add(fb.Lte(p => p.Precio, precioMax.Value));
            if (!string.IsNullOrWhiteSpace(proveedor))
                filtros.Add(fb.Eq(p => p.Proveedor.Nombre, proveedor));
            if (!string.IsNullOrWhiteSpace(etiqueta))
                filtros.Add(fb.AnyEq(p => p.Etiquetas, etiqueta));
            if (soloConStock)
                filtros.Add(fb.Gt(p => p.Stock, 0));

            var filtro = filtros.Count == 0 ? FilterDefinition<Producto>.Empty : fb.And(filtros);

            var sb = Builders<Producto>.Sort;
            SortDefinition<Producto> orden = ordenarPor?.ToLower() switch
            {
                "precio" => descendente ? sb.Descending(p => p.Precio) : sb.Ascending(p => p.Precio),
                "stock" => descendente ? sb.Descending(p => p.Stock) : sb.Ascending(p => p.Stock),
                _ => descendente ? sb.Descending(p => p.Nombre) : sb.Ascending(p => p.Nombre)
            };

            pagina = Math.Max(pagina, 1);
            tamano = Math.Clamp(tamano, 1, 100);

            var total = await _collection.CountDocumentsAsync(filtro);
            var items = await _collection.Find(filtro)
                .Sort(orden)
                .Skip((pagina - 1) * tamano)
                .Limit(tamano)
                .ToListAsync();

            return new PaginacionResult<Producto> { Result = items, Total = (int)total };
        }
    }
}