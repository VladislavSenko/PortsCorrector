using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortsProcessor.Data.Entities;
using PortsProcessor.Data.Repositories.Interfaces;
using PortsProcessor.Exceptions;

namespace PortsProcessor.Data.Repositories.Implementation
{
    class InputPortsRepository : IInputPortsRepository
    {
        private readonly PortsDbContext _portsDbContext;

        public InputPortsRepository(PortsDbContext portsDbContext)
        {
            _portsDbContext = portsDbContext;
        }

        public Task<List<InputPort>> GetNoProcessedPortsAsync()
        {
            var ports = _portsDbContext.InputPorts.Where(p => !p.IsProcessed).ToListAsync();
            return ports;
        }

        public async Task UpdateAsync(InputPort inputPort)
        {
            var port = await _portsDbContext.InputPorts.FirstOrDefaultAsync(p => p.Id == inputPort.Id);

            if(port == null)
                throw new EntityNotFoundException<PortCode>($"id: {inputPort.Id}");

            _portsDbContext.Update(inputPort);
            await _portsDbContext.SaveChangesAsync();
        }
    }
}
