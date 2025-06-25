using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LojaMoveis.Configurations;

public class TokenRedefinicaoService
{
    private readonly IMongoCollection<TokenRedefinicao> _colecao;

    public TokenRedefinicaoService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _colecao = database.GetCollection<TokenRedefinicao>("TokensRedefinicao");
    }

    public async Task CriarTokenAsync(string email, string token)
    {
        var novo = new TokenRedefinicao
        {
            Email = email,
            Token = token,
            ExpiraEm = DateTime.UtcNow.AddHours(1)
        };

        await _colecao.InsertOneAsync(novo);
    }

    public async Task<TokenRedefinicao> ObterPorTokenAsync(string token)
    {
        return await _colecao.Find(t => t.Token == token && t.ExpiraEm > DateTime.UtcNow).FirstOrDefaultAsync();
    }

    public async Task RemoverTokenAsync(string token)
    {
        await _colecao.DeleteOneAsync(t => t.Token == token);
    }
}
