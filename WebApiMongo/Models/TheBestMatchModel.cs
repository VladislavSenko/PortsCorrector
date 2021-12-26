using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMongo.Enums;

namespace WebApiMongo.Models
{
    public class TheBestMatchModel
    {
        public string PortCodeId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public SearchKindEnum SearchKind { get; set; }
        public decimal PortCodeMatchWeight { get; set; }
        public decimal PortNameMatchWeight { get; set; }
    }
}
