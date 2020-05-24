using System;
using System.Threading.Tasks;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.Minecraft;

namespace MinecraftServerStatus.Domain.Services
{
    public class PlayersCounterService
    {
        public async Task<CountRecord?> TryToGetCountRecord(string serverAddress)
        {
            var triesLeft = 5;
            while (triesLeft > 0)
            {
                try
                {
                    var countRecord = await GetCounts(serverAddress);
                    return countRecord;
                }
                catch (Exception e)
                {
                    triesLeft--;
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    await Task.Delay(3000);
                }
            }
            return null;
        }

        public async Task<CountRecord> GetCounts(string serverAddress)
        {
            var (onlinePlayers, slots) = await GetRealCount(serverAddress);
            var inQueue = onlinePlayers - slots;
            if (inQueue < 0)
            {
                inQueue = 0;
            }

            return new CountRecord
            {
                InQueuePlayers = inQueue,
                MaxSlots = slots,
                OnlinePlayers = onlinePlayers,
                ServerAddress = serverAddress
            };
        }

        private async Task<(int onlinePlayers, int slots)> GetRealCount(string serverAddress)
        {
            var serverPing = new ServerPing();
            var pingPayLoad = await serverPing.GetPingPayloadAsync(serverAddress); // possible throw exception if connection error
            return (pingPayLoad.Players.Online, pingPayLoad.Players.Max);
        }
    }
}
