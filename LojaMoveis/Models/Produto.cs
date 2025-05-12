using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LojaMoveis.Models
{
    public class Produto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public double Avaliacao { get; set; }
}
}
