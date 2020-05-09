using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MinecraftServerStatus.Domain.Integrations
{
    public class Session
    {
        private readonly IMongoDatabase _mongoDatabase;

        public Session(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IEnumerable<T> Get<T>() where T : Entity
        {
            return this.GetCollection<T>().AsQueryable();
        }

        public async Task AddAsync<T>(T item) where T : Entity
        {
            await this.GetCollection<T>().InsertOneAsync(item);
        }

        public async Task AddAsync<T>(IEnumerable<T> items) where T : Entity
        {
            await this.GetCollection<T>().InsertManyAsync(items);
        }

        private IMongoCollection<T> GetCollection<T>() where T : Entity
        {
            return _mongoDatabase.GetCollection<T>($"{typeof(T).Name}s");
        }
    }
}
