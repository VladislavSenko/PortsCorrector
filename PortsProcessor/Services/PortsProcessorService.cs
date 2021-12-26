using System.Linq;
using System.Threading.Tasks;
using PortsProcessor.Data.Repositories.Interfaces;
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
                if (string.IsNullOrWhiteSpace(inputPort.PortCode))
                {

                }
                else if (string.IsNullOrWhiteSpace(inputPort.PortName))
                {

                }
                else
                {
                    var portMatchResults = _matcherProvider.GetMatchCoefficient(inputPort, portCodes);
                    var theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults);
                    
                    var foundPorts = portCodes.Where(p => theBestMatches.Select(m => m.PortCodeId).Contains(p.Id))
                        .Reverse()
                        .ToList();
                    
                }
            }

            // var portMatchResults = _matcherProvider.GetMatchCoefficient(portNameOrCode, ports);
            //     var theBestMatches = _matcherProvider.GetTheBestMatch(portMatchResults);
            //
            //     var foundPorts = ports.Where(p => theBestMatches.Select(m => m.PortCodeId).Contains(p.PortCodeId))
            //         .Reverse()
            //         .ToList();
            //
            //     return foundPorts;
        }
    }
}
