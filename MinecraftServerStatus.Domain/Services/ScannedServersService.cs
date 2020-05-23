using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MinecraftServerStatus.Domain.Models;
using MinecraftServerStatus.Integrations.MongoDB;

namespace MinecraftServerStatus.Domain.Services
{
    public class ScannedServersService
    {
        private readonly SessionFactory _sessionFactory;
        private readonly List<string> _serverAddresses;

        public ScannedServersService(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _serverAddresses = new List<string>();
        }

        public async Task AddServer(string serverAddress)
        {
            if (_serverAddresses.Exists(x => x == serverAddress))
            {
                throw new Exception("Server already exists");
            }
            _serverAddresses.Add(serverAddress);
            var session = _sessionFactory.Create();
            await session.AddAsync(new Server(serverAddress));
        }

        public async Task RemoveServerAsync(string serverAddress)
        {
            _serverAddresses.Remove(serverAddress);
            var session = _sessionFactory.Create();
            await session.DeleteAsync(new Server(serverAddress), x => x.Address);
        }

        public IList<string> Servers => _serverAddresses;
    }
}
