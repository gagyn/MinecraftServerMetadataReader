using System;
using MinecraftServerStatus.Integrations.Minecraft;

namespace MinecraftServerStatus.Domain.Services
{
    public class ServerPlayersCounterService
    {
        private const string _HYPIXEL_IP = "mc.hypixel.net";

        public (int onlinePlayers, int slots) GetRealCount()
        {
            try
            {
                var serverPing = new ServerPing();
                var pingPayLoad = serverPing.GetPingPayloadAsync(_HYPIXEL_IP).Result;
                return (pingPayLoad.Players.Online, pingPayLoad.Players.Max);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return (0, 0);
            }
        }
    }
}
