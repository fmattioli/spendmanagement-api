namespace SpendManagement.Infra.CrossCutting.Conf
{
    public class TracingSettings
    {
        public string? Environment { get; set; }
        public string? Uri { get; set; }
        public int Port { get; set; }
    }
}
