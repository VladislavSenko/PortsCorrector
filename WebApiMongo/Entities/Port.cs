using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiMongo.Entities
{
    public class Port
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonPropertyName("PortCode")]
        [BsonElement("PortCode")] public string PortCode { get; set; }

        [JsonPropertyName("PortName")]
        [BsonElement("PortName")]
        public string[] PortName { get; set; }

        public Port()
        {
        }
    }
}
