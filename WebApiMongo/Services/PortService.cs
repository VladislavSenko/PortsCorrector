using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApiMongo.Entities;
using WebApiMongo.Models;
using WebApiMongo.Settings;

namespace WebApiMongo.Services
{
    public class PortService
    {
        private readonly IMongoCollection<Port> _portsCollection;
        private readonly MatcherProvider _matcherProvider;
        public PortService(
            IOptions<PortsStoreDatabaseSettings> portsStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                portsStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                portsStoreDatabaseSettings.Value.DatabaseName);

            _portsCollection = mongoDatabase.GetCollection<Port>(
                portsStoreDatabaseSettings.Value.PortsCollectionName);

            _matcherProvider = new MatcherProvider();
        }

        public async Task<List<Port>> GetAsync() =>
            await _portsCollection.Find(_ => true).ToListAsync();

        public async Task<Port?> GetAsync(string id) =>
            await _portsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Port newPort) =>
            await _portsCollection.InsertOneAsync(newPort);

        public async Task UpdateAsync(string id, Port updatedPort) =>
            await _portsCollection.ReplaceOneAsync(x => x.Id == id, updatedPort);

        public async Task RemoveAsync(string id) =>
            await _portsCollection.DeleteOneAsync(x => x.Id == id);
        
        public async Task<List<Port>> FindMatch(string portNameOrCode)
        {
            var ports = await GetAsync();
            var portMatchResults = _matcherProvider.GetMatchCoefficient(portNameOrCode, ports);
            var theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults);

            var foundPorts = ports.Where(p => theBestMatches.Select(m => m.PortId).Contains(p.Id))
                .Reverse()
                .ToList();

            return foundPorts;
        }
    }
}
