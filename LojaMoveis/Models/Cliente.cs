using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LojaMoveis.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("senha")]
        public string Senha { get; set; } = null!;

        [BsonElement("cpf")]
        public string Cpf { get; set; } = null!;

        [BsonElement("telefone")]
        public string Telefone { get; set; } = null!;
    }
}
