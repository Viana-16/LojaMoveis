using LojaMoveis.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using LojaMoveis.Configurations;

namespace LojaMoveis.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<Admin> _adminCollection;

        public AdminService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _adminCollection = mongoDatabase.GetCollection<Admin>("Admins");
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _adminCollection.Find(admin => admin.Email == email).FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(string id, Admin adminAtualizado)
        {
            await _adminCollection.ReplaceOneAsync(a => a.Id == id, adminAtualizado);
        }


        public async Task<Admin?> LoginAsync(string email, string senha)
        {
            // Busca o admin pelo email e senha
            var admin = await _adminCollection
                .Find(a => a.Email == email && a.Senha == senha)
                .FirstOrDefaultAsync();

            return admin;
        }
        

        public async Task CadastrarAdminAsync(Admin admin)
        {
            var existingAdmin = await GetByEmailAsync(admin.Email);
            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin com este email já existe.");
            }

            await _adminCollection.InsertOneAsync(admin);
        }
    }
}
