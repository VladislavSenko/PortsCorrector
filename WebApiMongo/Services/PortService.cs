using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApiMongo.Entities;
using WebApiMongo.Enums;
using WebApiMongo.Models;
using WebApiMongo.Settings;
using WebApiMongo.ViewModel;

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
        
        public async Task<List<TheBestMatchModel>> FindMatch(InputPortViewModel inputPortViewModel)
        {
            var searchKind = SearchKindEnum.PortCodeAndName;
            var theBestMatches = new List<TheBestMatchModel>();

            if (string.IsNullOrWhiteSpace(inputPortViewModel.PortCode))
                searchKind = SearchKindEnum.PortName;
            else if (string.IsNullOrWhiteSpace(inputPortViewModel.PortName))
                searchKind = SearchKindEnum.PortCode;

            var ports = await GetAsync();

            var port = ports.FirstOrDefault(p => p.PortCode == inputPortViewModel.PortCode &&
                                                 p.PortName.Contains(inputPortViewModel.PortName));
            if (port != null)
            {
                theBestMatches.Add(new TheBestMatchModel()
                {
                    PortName = port.PortName.FirstOrDefault(p => p == inputPortViewModel.PortName),
                    PortCodeId =  port.Id,
                    PortCodeMatchWeight =  1,
                    PortNameMatchWeight =  1,
                    SearchKind = SearchKindEnum.PortCodeAndName
                });

                return theBestMatches;
            }

            var portMatchResults = _matcherProvider.GetMatchCoefficient(inputPortViewModel, ports);
            theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults, searchKind);

            return theBestMatches;
        }
    }
}
