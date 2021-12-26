using Microsoft.EntityFrameworkCore;
using PortsProcessor.Data.Entities;

namespace PortsProcessor.Data
{
    class PortsDbContext : DbContext
    {
        public DbSet<InputPort> InputPorts { get; set; }
        public DbSet<PortCode> PortCodes{ get; set; }
        public DbSet<PortName>  PortNames { get; set; }
        public DbSet<ProcessedPort> ProcessedPorts{ get; set; }

        public PortsDbContext()
        {
        }

        public PortsDbContext(DbContextOptions<PortsDbContext> options)
            : base(options)
        {
        }
    }
}
