using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortsProcessor.Settings
{
    public class AppConfig
    {
        public int ReturnMatches { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PortsCollectionName { get; set; }
    }
}
