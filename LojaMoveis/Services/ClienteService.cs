using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LojaMoveis.Configurations;
using LojaMoveis.Models;
using BCrypt.Net;

namespace LojaMoveis.Services;

public class ClienteService
{
    private readonly IMongoCollection<Cliente> _clienteCollection;

    public ClienteService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _clienteCollection = mongoDatabase.GetCollection<Cliente>(settings.Value.ClienteCollectionName);
    }

    // Método para buscar um cliente pelo e-mail
    public async Task<Cliente?> GetByEmailAsync(string email)
    {
        return await _clienteCollection.Find(c => c.Email == email).FirstOrDefaultAsync();
    }

    // Método para pegar todos os clientes
    public async Task<List<Cliente>> GetAllAsync() =>
        await _clienteCollection.Find(_ => true).ToListAsync();

    // Método para buscar um cliente pelo ID
    public async Task<Cliente?> GetByIdAsync(string id) =>
        await _clienteCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

    // Método para criar um cliente (com senha criptografada)
    public async Task CreateAsync(Cliente cliente)
    {
        // Criptografando a senha do cliente antes de salvar
        cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
        await _clienteCollection.InsertOneAsync(cliente);
    }

    // Método para atualizar os dados de um cliente
    public async Task UpdateAsync(string id, Cliente cliente) =>
        await _clienteCollection.ReplaceOneAsync(x => x.Id == id, cliente);

    // Método para remover um cliente pelo ID
    public async Task RemoveAsync(string id) =>
        await _clienteCollection.DeleteOneAsync(x => x.Id == id);
}
