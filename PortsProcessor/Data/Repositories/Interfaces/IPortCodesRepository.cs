using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortsProcessor.Data.Entities;

namespace PortsProcessor.Data.Repositories.Interfaces
{
    public interface IPortCodesRepository
    {
        public Task<PortCode> GetPortCodeByIdAsync(int id);
        public Task<List<PortCode>> GetAllCodes();
    }
}
