using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsProcessor.Data.Entities
{
    public class PortCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public ICollection<PortName> PortNames { get; set; }
    }
}
