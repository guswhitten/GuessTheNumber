using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class PlayerModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Correct { get; set; }
    public int Attempts { get; set; }
    public double Accuracy { get; set; }
}