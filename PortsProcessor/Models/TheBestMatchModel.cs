using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortsProcessor.Models
{
    public class TheBestMatchModel
    {
        public string Value { get; set; }
        public int PortCodeId { get; set; }
        public bool isPortCode { get; set; }
        public decimal MatchScore { get; set; }
    }
}
