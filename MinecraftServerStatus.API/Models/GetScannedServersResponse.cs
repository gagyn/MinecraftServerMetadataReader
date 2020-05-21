using System.Collections.Generic;

namespace MinecraftServerStatus.API.Models
{
    public class GetScannedServersResponse
    {
        public IEnumerable<string> ServerNames { get; set; }
    }
}
