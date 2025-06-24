using JWT_Authentication_Sistemi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JWT_Authentication_Sistemi.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(
            IOptions<JWT_AuthenticationDBSettings> jWT_AuthenticationDBSettings)
        {
            var mongoClient = new MongoClient(
                jWT_AuthenticationDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                jWT_AuthenticationDBSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                jWT_AuthenticationDBSettings.Value.UsersCollectionName);
        }
        public async Task<List<User>> GetAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string email) =>
            await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string email, User updatedBook) =>
            await _usersCollection.ReplaceOneAsync(x => x.Email == email, updatedBook);

        public async Task RemoveAsync(string email) =>
            await _usersCollection.DeleteOneAsync(x => x.Email == email);
    }
}
