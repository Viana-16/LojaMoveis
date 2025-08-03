//using LojaMoveis.Configurations;
//using LojaMoveis.Models;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;

//namespace LojaMoveis.Services
//{
//    public class PedidoService
//    {
//        private readonly IMongoCollection<Pedido> _pedidoCollection;

//        public PedidoService(IOptions<MongoDbSettings> settings)
//        {
//            var client = new MongoClient(settings.Value.ConnectionString);
//            var db = client.GetDatabase(settings.Value.DatabaseName);
//            _pedidoCollection = db.GetCollection<Pedido>("Pedidos");
//        }

//        public async Task CriarAsync(Pedido pedido)
//        {
//            await _pedidoCollection.InsertOneAsync(pedido);
//        }

//        public async Task<List<Pedido>> GetPorUsuarioAsync(string email)
//        {
//            return await _pedidoCollection.Find(p => p.Email == email).ToListAsync();
//        }


//        public async Task<List<Pedido>> ObterPorEmail(string email)
//        {
//            return await _pedidoCollection
//                .Find(p => p.Email == email)
//                .ToListAsync();
//        }

//        public async Task<List<Pedido>> GetPorEmailAsync(string email)
//        {
//            return await _pedidoCollection
//                .Find(p => p.Email == email && (p.Status == "Pago" || p.Status == "Finalizado"))
//                .ToListAsync();
//        }


//    }
//}


using LojaMoveis.Configurations;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class PedidoService
{
    private readonly IMongoCollection<Pedido> _pedidoCollection;

    public PedidoService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        // Certifique-se de ter "PedidoCollectionName" no appsettings.json
        _pedidoCollection = mongoDatabase.GetCollection<Pedido>("Pedidos");
    }

    public async Task<List<Pedido>> GetPorUsuarioAsync(string email)
    {
        return await _pedidoCollection.Find(p => p.Email == email).ToListAsync();
    }

    public async Task<List<Pedido>> ObterPorEmail(string email)
    {
        return await _pedidoCollection
            .Find(p => p.Email == email)
            .ToListAsync();
    }

    public async Task Criar(Pedido novoPedido)
    {
        await _pedidoCollection.InsertOneAsync(novoPedido);
    }

    public async Task<Pedido> ObterPorId(string id)
    {
        return await _pedidoCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> Atualizar(string id, Pedido pedidoAtualizado)
    {
        var resultado = await _pedidoCollection.ReplaceOneAsync(p => p.Id == id, pedidoAtualizado);
        return resultado.ModifiedCount > 0;
    }
}
