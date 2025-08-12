using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LojaMoveis.Models
{
    public class Avaliacao
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("idProduto")]
        public string IdProduto { get; set; }

        [BsonElement("clienteEmail")]
        public string ClienteEmail { get; set; }  // <-- mudou aqui

        [BsonElement("nota")]
        public int Nota { get; set; }

        [BsonElement("comentario")]
        public string Comentario { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; }
    }
}
