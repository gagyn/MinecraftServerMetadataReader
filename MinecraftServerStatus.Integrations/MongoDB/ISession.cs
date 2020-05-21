using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServerStatus.Integrations.MongoDB
{
    public interface ISession
    {
        Task AddAsync<T>(IEnumerable<T> items) where T : Entity;
        Task AddAsync<T>(T item) where T : Entity;
        Task DeleteAsync<T>(T item) where T : Entity;
        IEnumerable<T> Get<T>() where T : Entity;
        Task ReplaceAsync<T>(T item) where T : Entity;
    }
}