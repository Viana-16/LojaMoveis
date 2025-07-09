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

    //Validação pra ver se CPF é real
    private bool ValidarCPF(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        var multiplicadores1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicadores2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
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
        // Verifica se já existe cliente com mesmo e-mail
        var existingEmail = await _clienteCollection.Find(c => c.Email == cliente.Email).FirstOrDefaultAsync();
        if (existingEmail != null)
            throw new Exception("E-mail já cadastrado.");

        if (!ValidarCPF(cliente.Cpf))
            throw new Exception("CPF inválido. Por favor, insira um CPF válido.");

        // Verifica se já existe cliente com mesmo CPF
        var existingCpf = await _clienteCollection.Find(c => c.Cpf == cliente.Cpf).FirstOrDefaultAsync();
        if (existingCpf != null)
            throw new Exception("CPF já cadastrado.");



        // Criptografa a senha antes de salvar
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