using System.Collections.Generic;
using System.Threading.Tasks;
using PortsProcessor.Data.Entities;

namespace PortsProcessor.Data.Repositories.Interfaces
{
    public interface IProcessedPortsRepository
    {
        public Task InsertRangeAsync(List<ProcessedPort> processedPorts);
    }      
}
