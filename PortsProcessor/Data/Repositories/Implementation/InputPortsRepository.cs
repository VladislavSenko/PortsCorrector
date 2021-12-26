using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortsProcessor.Data.Entities;
using PortsProcessor.Data.Repositories.Interfaces;

namespace PortsProcessor.Data.Repositories.Implementation
{
    class InputPortsRepository : IInputPortsRepository
    {
        private readonly DbSet<InputPort> _inputPorts;

        public InputPortsRepository(PortsDbContext dbContext)
        {
            _inputPorts = dbContext.InputPorts;
        }

        public Task<List<InputPort>> GetNoProcessedPortsAsync()
        {
            var ports = _inputPorts.Where(p => !p.IsProcessed).ToListAsync();
            return ports;
        }
    }
}
