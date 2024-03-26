namespace SpendManagement.Infra.CrossCutting.Conf
{
    public record KafkaSettings
    {
        public string Broker { get; set; } = null!;
        public string Environment { get; set; } = null!;
        public int RetryNumber { get; set; }
        public IEnumerable<string> Sasl_Brokers { get; set; } = null!;
        public bool Sasl_Enabled { get; set; }
        public string Sasl_Username { get; set; } = null!;
        public string Sasl_Password { get; set; } = null!;
        public string DependencyName { get; set; } = null!;
        public int ProducerRetryCount { get; set; } 
        public int ProducerRetryInterval { get; set; }
        public int MessageTimeoutMs { get; set; }
        public int ConsumerRetryCount { get; set; }
        public int ConsumerRetryInterval { get; set; }
        public string ConsumerInitialState { get; set; } = null!;
        public int WorkerCount { get; set; }
        public int BufferSize { get; set; }
        public KafkaBatchSettings Batch { get; set; } = null!;
    }
}
