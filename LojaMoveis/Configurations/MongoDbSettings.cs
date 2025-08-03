namespace LojaMoveis.Configurations
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProdutoCollectionName { get; set; } = "Produtos";
        public string ClienteCollectionName { get; set; } = "Clientes";
        public string PedidoCollectionName { get; set; }
    }
}
