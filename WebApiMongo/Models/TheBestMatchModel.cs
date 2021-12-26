using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMongo.Models
{
    public class TheBestMatchModel
    {
        public string Value { get; set; }
        public string PortId { get; set; }
        public bool isPortCode { get; set; }
        public decimal MatchScore { get; set; }
    }
}
