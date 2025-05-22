using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.Product.Repositories.Entities.Abstraction;

public abstract class MongoEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public virtual string Id { get; set; } = null!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? UpdatedDate { get; set; }
}