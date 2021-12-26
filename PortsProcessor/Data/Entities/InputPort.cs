namespace PortsProcessor.Data.Entities
{
    public class InputPort
    {
        public int Id { get; set; }
        public string PortName { get; set; }
        public string PortCode { get; set; }
        public bool IsProcessed { get; set; }
    }
}
