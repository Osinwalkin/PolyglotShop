using MongoDB.Driver;
using PolyglotShop.Core.Entities;
using PolyglotShop.Core.Interfaces;
using PolyglotShop.Infrastructure.Data;

namespace PolyglotShop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(MongoContext context)
    {
        _products = context.Products;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _products.Find(_ => true).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        await _products.InsertOneAsync(product);
    }
}