using LojaMoveis.Configurations;
using LojaMoveis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LojaMoveis.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<Admin> _adminCollection;

        public AdminService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _adminCollection = database.GetCollection<Admin>("Admins");
        }
        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _adminCollection.Find(a => a.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Admin?> LoginAsync(string email, string senha)
        {
            var admin = await _adminCollection.Find(a => a.Email == email).FirstOrDefaultAsync();

            if (admin is null || !BCrypt.Net.BCrypt.Verify(senha, admin.Senha))
            {
                return null;
            }

            return admin;
        }

        public async Task CadastrarAdminAsync(Admin admin)
        {
            admin.Senha = BCrypt.Net.BCrypt.HashPassword(admin.Senha);
            await _adminCollection.InsertOneAsync(admin);
        }
    }
}
