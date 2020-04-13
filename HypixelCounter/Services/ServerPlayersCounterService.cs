using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace HypixelCounter.Services
{
    public class ServerPlayersCounterService
    {
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

        private static string GetNumberFromPage()
        {
            const string hypixelUrl = "https://hypixel.net/";
            var client = new WebClient();
            client.Headers.Add("user-agent", "MyCounter/1.0");
            var content = client.DownloadString(hypixelUrl);
            var regex = new Regex("Join <b>(?<playersCount>[\\d,]+)<\\/b>");
            var playersCount = regex.Match(content).Groups["playersCount"].Value;
            return playersCount;
        }
    }
}
