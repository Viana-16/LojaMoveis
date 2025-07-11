﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LojaMoveis.Models
{
    public class ResetToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Token")]
        public string Token { get; set; }

        [BsonElement("ExpiraEm")]
        public DateTime ExpiraEm { get; set; }
    }
}
