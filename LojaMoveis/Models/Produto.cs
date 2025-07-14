//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson;
//using System.ComponentModel.DataAnnotations;

//namespace LojaMoveis.Models
//{
//    [BsonIgnoreExtraElements]
//    public class Produto
//    {
//        [BsonId]
//        [BsonRepresentation(BsonType.ObjectId)]
//        public string? Id { get; set; }

//        [Required]
//        public string Nome { get; set; }

//        [Required]
//        public decimal Preco { get; set; }

//        public string Descricao { get; set; }

//        public string Categoria { get; set; }

//        public string? ImagemUrl { get; set; }

//    }
//}



using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace LojaMoveis.Models
{
    [BsonIgnoreExtraElements]
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public string Descricao { get; set; }

        public string Categoria { get; set; }

        public string? ImagemUrl { get; set; }
        public List<string> ImagensExtras { get; set; } = new List<string>(); // novas imagens extras
    }
}