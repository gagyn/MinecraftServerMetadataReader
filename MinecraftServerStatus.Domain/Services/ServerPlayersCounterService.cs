using MinecraftServerStatus.Integrations.Minecraft;

namespace MinecraftServerStatus.Domain.Services
{
    public class ServerPlayersCounterService
    {
        private const string _HYPIXEL_IP = "mc.hypixel.net"; // todo: 

        public (int onlinePlayers, int slots) GetRealCount()
        {
            var serverPing = new ServerPing();
            var pingPayLoad = serverPing.GetPingPayloadAsync(_HYPIXEL_IP).Result; // possible throw exception if connection error
            return (pingPayLoad.Players.Online, pingPayLoad.Players.Max);
        }
    }
}
