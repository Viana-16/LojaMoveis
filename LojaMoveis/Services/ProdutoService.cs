using MongoDB.Driver;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using LojaMoveis.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LojaMoveis.Services
{
    public class ProdutoService
    {
        private readonly IMongoCollection<Produto> _produtoCollection;

        public ProdutoService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _produtoCollection = mongoDatabase.GetCollection<Produto>("Produtos");
        }

        public async Task<List<Produto>> GetAsync() =>
            await _produtoCollection.Find(_ => true).ToListAsync();

        public async Task<Produto?> GetByIdAsync(string id) =>
            await _produtoCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        public async Task CadastrarProdutoAsync(Produto produto)
        {
            await _produtoCollection.InsertOneAsync(produto);
        }

        public async Task CreateAsync(Produto produto) =>
            await _produtoCollection.InsertOneAsync(produto);

        public async Task UpdateAsync(string id, Produto produtoAtualizado) =>
            await _produtoCollection.ReplaceOneAsync(p => p.Id == id, produtoAtualizado);

        public async Task DeleteAsync(string id) =>
            await _produtoCollection.DeleteOneAsync(p => p.Id == id);
    }
}
