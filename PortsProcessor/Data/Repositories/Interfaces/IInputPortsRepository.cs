using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortsProcessor.Data.Entities;

namespace PortsProcessor.Data.Repositories.Interfaces
{
    public interface IInputPortsRepository
    {
        public Task<List<InputPort>> GetNoProcessedPortsAsync();
        public Task UpdateAsync(InputPort inputPort);
    } 
}
