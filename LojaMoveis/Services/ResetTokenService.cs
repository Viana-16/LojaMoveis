using LojaMoveis.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LojaMoveis.Services
{
    public class ResetTokenService
    {
        //private readonly IMongoCollection<ResetToken> _resetTokens;

        //public ResetTokenService(IConfiguration config)
        //{
        //    var client = new MongoClient(config.GetConnectionString("MongoDb"));
        //    var database = client.GetDatabase("LojaMoveisDB");
        //    _resetTokens = database.GetCollection<ResetToken>("ResetTokens");
        //}

        private readonly IMongoCollection<ResetToken> _resetTokens;

        public ResetTokenService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
            _resetTokens = database.GetCollection<ResetToken>("ResetTokens");
        }


        public async Task CriarAsync(ResetToken token)
        {
            await _resetTokens.InsertOneAsync(token);
        }

        public async Task<ResetToken> ObterPorTokenAsync(string token)
        {
            return await _resetTokens.Find(t => t.Token == token && t.ExpiraEm > DateTime.UtcNow)
                                     .FirstOrDefaultAsync();
        }

        public async Task RemoverTokenAsync(string token)
        {
            await _resetTokens.DeleteOneAsync(t => t.Token == token);
        }
    }
}
