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
    class PortNamesRepository : IPortNamesRepository
    {
        private readonly DbSet<PortName> _portNames;

        public PortNamesRepository(PortsDbContext dbContext)
        {
            _portNames = dbContext.PortNames;
        }
        public async Task<PortName> GetPortNameByIdAsync(int id)
        {
            var portName = await _portNames.FirstOrDefaultAsync(p => p.Id == id);

            if (portName == null)
                throw new EntityNotFoundException<PortName>($"id: {id}");

            return portName;
        }

        public async Task<List<PortName>> GetAllNamesAsync()
        {
            var portNames = await _portNames.ToListAsync();

            return portNames;
        }
    }
}
