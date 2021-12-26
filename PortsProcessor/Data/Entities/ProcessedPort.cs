using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsProcessor.Data.Entities
{
    public class ProcessedPort
    {
        public int Id { get; set; }
        public int InputPortId { get; set; }
        public int PortCodeId { get; set; }
        public int PortNameId { get; set; }
        public decimal MatchWeight { get; set; }

        [ForeignKey("InputPortId")]
        public InputPort InputPort { get; set; }

        [ForeignKey("PortCodeId")]
        public PortCode PortCode { get; set; }
        
        [ForeignKey("PortNameId")]
        public PortName PortName{ get; set; }
    }
}
