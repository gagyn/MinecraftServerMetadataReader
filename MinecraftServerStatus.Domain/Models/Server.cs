using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Models
{
    internal class Server : Entity
    {
        public string Address { get; set; }

        public Server(string address)
        {
            this.Address = address;
        }
    }
}
