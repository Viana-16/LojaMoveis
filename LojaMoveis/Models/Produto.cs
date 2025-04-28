using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LojaMoveis.Models
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; } = null!;

        [BsonElement("preco")]
        public decimal Preco { get; set; }

        [BsonElement("categoria")]
        public string Categoria { get; set; } = null!;

        [BsonElement("descricao")]
        public string? Descricao { get; set; }

        [BsonElement("imagem")]
        public string? Imagem { get; set; }
    }
}
