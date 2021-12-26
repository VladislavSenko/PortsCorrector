using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortsProcessor.Data.Entities;
using PortsProcessor.Data.Repositories.Interfaces;
using PortsProcessor.Exceptions;

namespace PortsProcessor.Data.Repositories.Implementation
{
    class PortCodesRepository : IPortCodesRepository
    {
        private readonly DbSet<PortCode> _portCodes;

        public PortCodesRepository(PortsDbContext dbContext)
        {
            _portCodes = dbContext.PortCodes;
        }
        public async Task<PortCode> GetPortCodeByIdAsync(int id)
        {
            var portCode = await _portCodes.FirstOrDefaultAsync(p => p.Id == id);

            if (portCode == null)
                throw new EntityNotFoundException<PortCode>($"id: {id}");

            return portCode;
        }

        public async Task<List<PortCode>> GetAllCodes()
        {
            var portCodes = await _portCodes.Include(c => c.PortNames).ToListAsync();

            return portCodes;
        }

        public async Task<PortCode> GetPortCodeByCode(string code)
        {
            var portCode = await _portCodes.Include(p => p.PortNames)
                .FirstOrDefaultAsync(p => p.Code.Equals(code));

            return portCode;
        }
    }
}
