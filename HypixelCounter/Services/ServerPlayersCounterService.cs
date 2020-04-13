using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace HypixelCounter.Services
{
    public class ServerPlayersCounterService
    {
        private const string _HYPIXEL_URL = "https://hypixel.net/";
        private const string _HYPIXEL_IP = "mc.hypixel.net";

        /// <summary>
        /// Gets number of players now on server or -1 if error
        /// </summary>
        /// <returns>Number of players now on server or -1 if error</returns>
        public int GetCount()
        {
            var playersCount = GetNumberFromPage();
            var success = int.TryParse(playersCount, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var parsedPlayersCount);
            return success ? parsedPlayersCount : -1;
        }

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

        private static string GetNumberFromPage()
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "MyCounter/1.0");
            var content = client.DownloadString(_HYPIXEL_URL);
            var regex = new Regex("Join <b>(?<playersCount>[\\d,]+)<\\/b>");
            var playersCount = regex.Match(content).Groups["playersCount"].Value;
            return playersCount;
        }
    }
}
