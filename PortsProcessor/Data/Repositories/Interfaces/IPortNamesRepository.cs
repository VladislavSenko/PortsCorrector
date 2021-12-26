using System.Collections.Generic;
using System.Threading.Tasks;
using PortsProcessor.Data.Entities;

namespace PortsProcessor.Data.Repositories.Interfaces
{
    public interface IPortNamesRepository
    {
        public Task<PortName> GetPortNameByIdAsync(int id);
        public Task<List<PortName>> GetAllNamesAsync();
    }
}
