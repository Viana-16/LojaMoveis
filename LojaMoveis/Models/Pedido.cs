//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

//namespace LojaMoveis.Models
//{
//    public class Pedido
//    {
//        [BsonElement("email")]
//        public string Email { get; set; }

//        [BsonElement("produtos")]
//        public List<ItemPedido> Produtos { get; set; }

//        [BsonElement("total")]
//        public double Total { get; set; }

//        [BsonElement("dataPedido")]
//        public DateTime DataPedido { get; set; }

//        [BsonElement("status")]
//        public string Status { get; set; }
//    }

//    public class ItemPedido
//    {
//        [BsonElement("produtoId")]
//        public string ProdutoId { get; set; }

//        [BsonElement("nome")]
//        public string Nome { get; set; }

//        [BsonElement("preco")]
//        public double Preco { get; set; }

//        [BsonElement("quantidade")]
//        public int Quantidade { get; set; }
//    }
//}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Pedido
{
    //[BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    //public string Id { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("produtos")]
    public List<ItemPedido> Produtos { get; set; }

    [BsonElement("total")]
    public decimal Total { get; set; }

    [BsonElement("dataPedido")]
    public DateTime DataPedido { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }
}

public class ItemPedido
{
    [BsonElement("produtoId")]
    public string ProdutoId { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; }

    [BsonElement("preco")]
    public decimal Preco { get; set; }

    [BsonElement("quantidade")]
    public int Quantidade { get; set; }
}
