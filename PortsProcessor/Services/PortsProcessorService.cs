using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortsProcessor.Data.Entities;
using PortsProcessor.Data.Repositories.Interfaces;
using PortsProcessor.Enums;
using PortsProcessor.Models;
using PortsProcessor.Providers;

namespace PortsProcessor.Services
{
    public class PortsProcessorService
    {
        private readonly IInputPortsRepository _inputPortsRepository;
        private readonly IPortCodesRepository _portCodesRepository;
        private readonly IProcessedPortsRepository _processedPortsRepository;
        private readonly MatcherProvider _matcherProvider;

        public PortsProcessorService(IInputPortsRepository inputPortsRepository, 
            IPortCodesRepository portCodesRepository, 
            IProcessedPortsRepository processedPortsRepository, MatcherProvider matcherProvider)
        {
            _inputPortsRepository = inputPortsRepository;
            _portCodesRepository = portCodesRepository;
            _processedPortsRepository = processedPortsRepository;
            _matcherProvider = matcherProvider;
        }

        public async Task StartProcessing()
        {
            var inputPorts = await _inputPortsRepository.GetNoProcessedPortsAsync();
            var portCodes = await _portCodesRepository.GetAllCodes();

            foreach (var inputPort in inputPorts)
            {
                var foundPorts = new List<ProcessedPort>();

                var portCode = await _portCodesRepository.GetPortCodeByCode(inputPort.PortCode);
                var portName = portCode?.PortNames.FirstOrDefault(p => p.Name == inputPort.PortName);

                if (portCode != null && portCode != null)
                {
                    foundPorts.Add(new ProcessedPort()
                    {
                        InputPortId = inputPort.Id,
                        PortCodeId = portCode.Id,
                        PortNameId = portName.Id,
                        MatchWeight = 2
                    });
                }
                else
                {
                    var portMatchResults = _matcherProvider.GetMatchCoefficient(inputPort, portCodes);

                    var theBestMatches = new List<TheBestMatchModel>();

                    if (string.IsNullOrWhiteSpace(inputPort.PortCode))
                    {
                        theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults, SearchKindEnum.PortName);

                        foundPorts = theBestMatches.Select(p => new ProcessedPort()
                        {
                            PortNameId = p.PortNameId,
                            PortCodeId = p.PortCodeId,
                            InputPortId = inputPort.Id,
                            MatchWeight = p.PortNameMatchWeight
                        }).ToList();
                    }
                    else if (string.IsNullOrWhiteSpace(inputPort.PortName))
                    {
                        theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults, SearchKindEnum.PortCode);

                        foundPorts = theBestMatches.Select(p => new ProcessedPort()
                        {
                            PortNameId = p.PortNameId,
                            PortCodeId = p.PortCodeId,
                            InputPortId = inputPort.Id,
                            MatchWeight = p.PortCodeMatchWeight
                        }).ToList();
                    }
                    else
                    {
                        theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults, SearchKindEnum.PortCodeAndName);

                        foundPorts = theBestMatches.Select(p => new ProcessedPort()
                        {
                            PortNameId = p.PortNameId,
                            PortCodeId = p.PortCodeId,
                            InputPortId = inputPort.Id,
                            MatchWeight = p.PortCodeMatchWeight + p.PortNameMatchWeight
                        }).ToList();
                    }

                }

                await _processedPortsRepository.InsertRangeAsync(foundPorts);

                inputPort.IsProcessed = true;
                await _inputPortsRepository.UpdateAsync(inputPort);
            }
        }
    }
}
