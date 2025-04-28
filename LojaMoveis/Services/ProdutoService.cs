using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using LojaMoveis.Configurations;
using LojaMoveis.Models;

namespace LojaMoveis.Services;

public class ProdutoService
{
    private readonly IMongoCollection<Produto> _produtos;

    public ProdutoService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _produtos = database.GetCollection<Produto>(settings.Value.ProdutoCollectionName);
    }

    public async Task<List<Produto>> GetAsync() =>
        await _produtos.Find(p => true).ToListAsync();

    public async Task<Produto?> GetByIdAsync(string id) =>
        await _produtos.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Produto produto)
    {
        if (string.IsNullOrEmpty(produto.Id))
        {
            produto.Id = ObjectId.GenerateNewId().ToString();
        }
        await _produtos.InsertOneAsync(produto);
    }

    public async Task UpdateAsync(string id, Produto produto) =>
        await _produtos.ReplaceOneAsync(p => p.Id == id, produto);

    public async Task DeleteAsync(string id) =>
        await _produtos.DeleteOneAsync(p => p.Id == id);
}
