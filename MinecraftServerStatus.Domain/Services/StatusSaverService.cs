using System.Threading.Tasks;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Services
{
    public class StatusSaverService
    {
        private readonly SessionFactory _sessionFactory;

        public StatusSaverService(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Write(int onlinePlayers, int inQueue, int slots, string serverAddress)
        {
            var countRecord = new CountRecord
            {
                OnlinePlayers = onlinePlayers,
                InQueuePlayers = inQueue,
                MaxSlots = slots
            };
            await Write(countRecord);
        }

        public async Task Write(CountRecord countRecord)
        {
            var session = _sessionFactory.Create();
            await session.AddAsync(countRecord);
        }
    }
}
