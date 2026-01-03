using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PolyglotShop.Core.Entities;

namespace PolyglotShop.Infrastructure.Data;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration config)
    {
        // U46: Distributed DB Connection
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("ShopCatalog");
    }

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
}