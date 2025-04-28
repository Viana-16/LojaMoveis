using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LojaMoveis.Configurations;
using LojaMoveis.Models;

namespace LojaMoveis.Services;

public class ClienteService
{
    private readonly IMongoCollection<Cliente> _clientes;

    public ClienteService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _clientes = database.GetCollection<Cliente>(settings.Value.ClienteCollectionName);
    }

    public async Task<List<Cliente>> GetAsync() =>
        await _clientes.Find(c => true).ToListAsync();

    public async Task<Cliente?> GetByEmailAsync(string email) =>
        await _clientes.Find(c => c.Email == email).FirstOrDefaultAsync();

    public async Task<Cliente?> GetByIdAsync(string id) =>
        await _clientes.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Cliente cliente) =>
        await _clientes.InsertOneAsync(cliente);
}
