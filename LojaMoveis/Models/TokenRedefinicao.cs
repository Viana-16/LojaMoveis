using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LojaMoveis.Models
{
    public class TokenRedefinicao
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}
