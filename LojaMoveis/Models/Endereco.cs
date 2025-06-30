using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LojaMoveis.Models
{
    public class Endereco
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // ← torna nulo


        [BsonElement("usuarioId")]
        public string UsuarioId { get; set; }

        [BsonElement("endereco")]
        public string TextoEndereco { get; set; }
    }
}
