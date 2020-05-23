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
            return GetCollection<T>().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAsync<T>() where T : Entity
        {
            return await Task.Run(() => GetCollection<T>().AsQueryable());
        }

        public async Task ReplaceByTypeAsync<T>(T item) where T : Entity
        {
            var possiblePreviousItems = Get<T>().ToList();
            if (possiblePreviousItems.Count > 1)
            {
                throw new Exception("To replace there must be only one item of this type");
            }
            if (possiblePreviousItems.Count == 1)
            {
                await DeleteAsync(possiblePreviousItems.First());
            }
            await AddAsync(item);
        }

        public async Task ReplaceAsync<T>(T oldItem, T newItem) where T : Entity
        {
            await DeleteAsync(oldItem);
            await AddAsync(newItem);
        }

        public async Task UpdateAsync<T>(T item) where T : Entity
        {
            await GetCollection<T>().ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task AddAsync<T>(T item) where T : Entity
        {
            await GetCollection<T>().InsertOneAsync(item);
        }

        public async Task AddAsync<T>(IEnumerable<T> items) where T : Entity
        {
            await GetCollection<T>().InsertManyAsync(items);
        }

        public async Task DeleteAsync<T>(T item) where T : Entity
        {
            await GetCollection<T>().DeleteOneAsync(x => x.Id == item.Id);
        }

        public async Task DeleteAsync<T, TW>(T item, Func<T, TW> by) where T : Entity
        {
            var items = await GetAsync<T>();
            var foundItems = items
                .Where(x => by.Invoke(x).Equals(by.Invoke(item)))
                .ToList();

            if (foundItems.Count > 1)
            {
                throw new Exception($"Too many the same items, you need to specify {nameof(by)}");
            }
            if (foundItems.Count == 0)
            {
                throw new Exception("The item doesn't exist");
            }

            await DeleteAsync(foundItems.First());
        }

        private IMongoCollection<T> GetCollection<T>() where T : Entity
        {
            return _mongoDatabase.GetCollection<T>($"{typeof(T).Name}s");
        }
    }
}
