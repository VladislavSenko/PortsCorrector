using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PortsProcessor.Data.Entities;
using PortsProcessor.Providers;
using PortsProcessor.Settings;

namespace PortsProcessor.Services
{
    public class PortService
    {
        private readonly IMongoCollection<Port> _portsCollection;
        private readonly MatcherProvider _matcherProvider;
        public PortService()
        {
            var mongoClient = new MongoClient(AppSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(AppSettings.DatabaseName);

            _portsCollection = mongoDatabase.GetCollection<Port>(AppSettings.PortsCollectionName);
            _matcherProvider = new MatcherProvider();
        }

        public async Task<List<Port>> GetAsync() =>
            await _portsCollection.Find(_ => true).ToListAsync();

        public async Task<Port?> GetAsync(string id) =>
            await _portsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // public async Task<List<Port>> FindMatch(string portNameOrCode)
        // {
        //     var ports = await GetAsync();
        //     var portMatchResults = _matcherProvider.GetMatchCoefficient(portNameOrCode, ports);
        //     var theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults);
        //
        //     var foundPorts = ports.Where(p => theBestMatches.Select(m => m.PortCodeId).Contains(p.PortCodeId))
        //         .Reverse()
        //         .ToList();
        //
        //     return foundPorts;
        // }
    }
}
