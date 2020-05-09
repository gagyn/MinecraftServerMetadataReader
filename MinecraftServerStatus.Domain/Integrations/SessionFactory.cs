using MongoDB.Driver;

namespace MinecraftServerStatus.Domain.Integrations
{
    public class SessionFactory
    {
        private readonly IMongoDatabase _mongoDatabase;

        public SessionFactory(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public Session Create()
        {
            return new Session(_mongoDatabase);
        }
    }
}
