namespace SpendManagement.Integration.Tests.Configuration
{
    public sealed class KafkaSettings
    {
        public IEnumerable<string> Brokers { get; set; } = null!;

        public string Environment { get; set; } = null!;

        public int InitializationDelay { get; set; }

        public KafkaBatchSettings? Batch { get; set; }

    }

    public class KafkaBatchSettings
    {
        public int WorkerCount { get; set; }

        public int BufferSize { get; set; }

        public int MessageTimeoutSec { get; set; }
    }
}
