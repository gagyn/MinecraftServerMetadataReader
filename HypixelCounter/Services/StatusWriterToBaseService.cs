using System.Threading.Tasks;
using HypixelCounter.Integrations;
using HypixelCounter.Models;

namespace HypixelCounter.Services
{
    public class StatusWriterToBaseService
    {
        private readonly SessionFactory _sessionFactory;

        public StatusWriterToBaseService(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task WriteToBase(int onlinePlayers, int inQueue, int slots)
        {
            var session = _sessionFactory.Create();
            var countRecord = new CountRecord
            {
                OnlinePlayers = onlinePlayers,
                InQueuePlayers = inQueue,
                MaxSlots = slots
            };
            await session.AddAsync(countRecord);
        }
    }
}
