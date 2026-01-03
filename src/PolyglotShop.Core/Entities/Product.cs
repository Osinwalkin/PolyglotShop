using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolyglotShop.Core.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // U40: Flexible Schema. Denne dictionary kan holde alt data
    [BsonExtraElements]
    public Dictionary<string, object> Details { get; set; } = new();
}