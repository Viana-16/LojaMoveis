using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LojaMoveis.Models
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Nome { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = null!;
    }
}
