using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MinecraftServerStatus.Integrations.MongoDB
{
    public class Session : ISession
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

        public async Task ReplaceByTypeAsync<T>(T item) where T : Entity
        {
            var possiblePreviousItems = this.Get<T>().ToList();
            if (possiblePreviousItems.Count > 1)
            {
                throw new Exception("To replace there must be only one item of this type");
            }
            if (possiblePreviousItems.Count == 1)
            {
                await this.DeleteAsync(possiblePreviousItems.First());
            }
            await this.AddAsync(item);
        }

        public async Task ReplaceAsync<T>(T oldItem, T newItem) where T : Entity
        {
            await this.DeleteAsync(oldItem);
            await this.AddAsync(newItem);
        }

        public async Task UpdateAsync<T>(T item) where T : Entity
        {
            await this.GetCollection<T>().ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task AddAsync<T>(T item) where T : Entity
        {
            await this.GetCollection<T>().InsertOneAsync(item);
        }

        public async Task AddAsync<T>(IEnumerable<T> items) where T : Entity
        {
            await this.GetCollection<T>().InsertManyAsync(items);
        }

        public async Task DeleteAsync<T>(T item) where T : Entity
        {
            await this.GetCollection<T>().DeleteOneAsync(x => x.Id == item.Id);
        }

        private IMongoCollection<T> GetCollection<T>() where T : Entity
        {
            return _mongoDatabase.GetCollection<T>($"{typeof(T).Name}s");
        }
    }
}
