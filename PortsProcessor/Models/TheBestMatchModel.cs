using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortsProcessor.Enums;

namespace PortsProcessor.Models
{
    public class TheBestMatchModel
    {
        public int PortCodeId { get; set; }
        public int PortNameId { get; set; }
        public SearchMatchKindEnum SearchMatchKind { get; set; }
        public decimal PortCodeMatchWeight { get; set; }
        public decimal PortNameMatchWeight { get; set; }
    }
}
