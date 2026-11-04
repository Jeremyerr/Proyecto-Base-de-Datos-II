using InventarioAPI.Models;
using MongoDB.Driver;

namespace InventarioAPI.Data.Seed
{
    public static class DataSeeder
    {
        public async static Task SeedAsync(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<ApplicationUser>("usuarios");

            var creador = await collection.Find(u => u.Email == "creador@api.com").FirstOrDefaultAsync();
            if (creador == null)
            {
                await collection.InsertOneAsync(new ApplicationUser { Email = "creador@api.com", Password = "Examen123!", Rol = "Creador" });
                await collection.InsertOneAsync(new ApplicationUser { Email = "visualizador@api.com", Password = "Examen123!", Rol = "Visualizador" });
            }
        }
    }
}