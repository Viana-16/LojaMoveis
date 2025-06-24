using LojaMoveis.Configurations;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

public class ResetSenhaService
{
    private readonly IMongoCollection<ResetSenhaToken> _tokens;

    public ResetSenhaService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _tokens = database.GetCollection<ResetSenhaToken>("ResetSenhaTokens");
    }

    public async Task CriarTokenAsync(string email, string token)
    {
        var reset = new ResetSenhaToken
        {
            Email = email,
            Token = token,
            Expiracao = DateTime.UtcNow.AddHours(1)
        };

        await _tokens.InsertOneAsync(reset);
    }

    public async Task<ResetSenhaToken> ValidarTokenAsync(string token)
    {
        return await _tokens.Find(t => t.Token == token && t.Expiracao > DateTime.UtcNow).FirstOrDefaultAsync();
    }

    public async Task RemoverTokenAsync(string token)
    {
        await _tokens.DeleteOneAsync(t => t.Token == token);
    }
}
