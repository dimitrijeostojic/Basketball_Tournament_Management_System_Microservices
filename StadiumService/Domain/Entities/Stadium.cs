using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Stadium
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("publicId")]
    [BsonRepresentation(BsonType.String)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    [BsonElement("stadiumName")]
    public string StadiumName { get; set; } = string.Empty;

    [BsonElement("city")]
    public string City { get; set; } = string.Empty;

    [BsonElement("capacity")]
    public int Capacity { get; set; }

    public static Stadium Create(string stadiumName, string city, int capacity)
    {
        return new Stadium
        {
            Id = ObjectId.GenerateNewId().ToString(),
            StadiumName = stadiumName,
            City = city,
            Capacity = capacity
        };
    }
}
