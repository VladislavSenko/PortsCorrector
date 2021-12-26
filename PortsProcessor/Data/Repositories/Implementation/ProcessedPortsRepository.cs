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
    class ProcessedPortsRepository : IProcessedPortsRepository
    {
        private PortsDbContext _dbContext;

        public ProcessedPortsRepository(PortsDbContext portsDbContext)
        {
            _dbContext = portsDbContext;
        }

        public async Task InsertRangeAsync(List<ProcessedPort> processedPorts)
        {
            await _dbContext.AddRangeAsync(processedPorts);
            await _dbContext.SaveChangesAsync();
        }
    }
}
