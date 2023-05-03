using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("Category")]
    public string Category { get; set; } = string.Empty;
    [BsonElement("Summary")]
    public string Summary { get; set; } = string.Empty;
    [BsonElement("Description")]
    public string Description { get; set; } = string.Empty;
    [BsonElement("ImageFile")]
    public string ImageFile { get; set; } = string.Empty;
    [BsonElement("Price")]
    public decimal Price { get; set; }
    [BsonElement("Quantity")]
    public int Quantity { get; set; }
}
