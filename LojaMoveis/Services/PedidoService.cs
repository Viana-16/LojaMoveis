using LojaMoveis.Configurations;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LojaMoveis.Services
{
    public class PedidoService
    {
        private readonly IMongoCollection<Pedido> _pedidoCollection;

        public PedidoService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _pedidoCollection = db.GetCollection<Pedido>("Pedidos");
        }

        public async Task CriarAsync(Pedido pedido)
        {
            await _pedidoCollection.InsertOneAsync(pedido);
        }

        public async Task<List<Pedido>> GetPorUsuarioAsync(string email)
        {
            return await _pedidoCollection.Find(p => p.Email == email).ToListAsync();
        }
    }
}
