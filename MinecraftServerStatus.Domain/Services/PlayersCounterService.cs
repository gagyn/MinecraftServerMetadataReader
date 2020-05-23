using System;
using System.Threading.Tasks;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.Minecraft;

namespace MinecraftServerStatus.Domain.Services
{
    public class PlayersCounterService
    {
        private const string _HYPIXEL_IP = "mc.hypixel.net"; // todo: 

        public bool TryToGetCounts(string serverAddress, out CountRecord? countRecord)
        {
            var triesLeft = 5;
            while (triesLeft > 0)
            {
                try
                {
                    countRecord = GetCounts(serverAddress);
                    return true;
                }
                catch (Exception e)
                {
                    triesLeft--;
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    Task.Delay(3000).Wait();
                }
            }
            countRecord = null;
            return false;
        }

        public CountRecord GetCounts(string serverAddress)
        {
            var (onlinePlayers, slots) = GetRealCount(serverAddress);
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

        private (int onlinePlayers, int slots) GetRealCount(string serverAddress)
        {
            var serverPing = new ServerPing();
            var pingPayLoad = serverPing.GetPingPayloadAsync(serverAddress).Result; // possible throw exception if connection error
            return (pingPayLoad.Players.Online, pingPayLoad.Players.Max);
        }
    }
}
