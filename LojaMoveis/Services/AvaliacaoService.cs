using LojaMoveis.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LojaMoveis.Services
{
    public class AvaliacaoService
    {
        private readonly IMongoCollection<Avaliacao> _avaliacoes;

        public AvaliacaoService(IMongoDatabase database)
        {
            _avaliacoes = database.GetCollection<Avaliacao>("Avaliacoes");
        }

        public async Task<List<Avaliacao>> GetPorProdutoAsync(string idProduto)
        {
            return await _avaliacoes.Find(a => a.IdProduto == idProduto).ToListAsync();
        }

        public async Task<Avaliacao> CriarAsync(Avaliacao novaAvaliacao)
        {
            await _avaliacoes.InsertOneAsync(novaAvaliacao);
            return novaAvaliacao;
        }

        public async Task<bool> JaAvaliouAsync(string idProduto, string email)
        {
            var count = await _avaliacoes.CountDocumentsAsync(a => a.IdProduto == idProduto && a.ClienteEmail == email);
            return count > 0;
        }

    }
}
