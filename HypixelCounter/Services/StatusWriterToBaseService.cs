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

        public async Task WriteToBase(int playersCount, int onlinePlayers, int slots)
        {
            var session = _sessionFactory.Create();
            var countRecord = new CountRecord
            {
                PlayersCount = playersCount,
                OnlinePlayers = onlinePlayers,
                MaxSlots = slots
            };
            await session.AddAsync(countRecord);
        }
    }
}
