using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LojaMoveis.Models
{
    public class Admin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("senha")]
        public string Senha { get; set; } = null!;
    }
}
