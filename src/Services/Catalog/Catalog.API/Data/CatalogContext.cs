using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration settings)
    {
        var client = new MongoClient(settings.GetValue<string>("DatabaseSettings : ConnectionString"));
        var database = client.GetDatabase(settings.GetValue<string>("DatabaseSettings : DatabaseName"));
        Products = database.GetCollection<Product>(settings.GetValue<string>("DatabaseSettings : CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }
    public IMongoCollection<Product> Products { get; }
}
