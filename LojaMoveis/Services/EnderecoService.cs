using LojaMoveis.Configurations;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LojaMoveis.Services
{
    public class EnderecoService
    {
        private readonly IMongoCollection<Endereco> _enderecoCollection;

        public EnderecoService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _enderecoCollection = database.GetCollection<Endereco>("Enderecos");
        }

        public async Task<List<Endereco>> GetPorUsuario(string usuarioId) =>
            await _enderecoCollection.Find(e => e.UsuarioId == usuarioId).ToListAsync();

        public async Task<Endereco> Criar(Endereco endereco)
        {
            await _enderecoCollection.InsertOneAsync(endereco);
            return endereco;
        }
    }
}
