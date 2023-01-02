namespace Spents.Infra.CrossCutting.Conf
{
    public record KafkaSettings
    {
        public KafkaSettings(string brokers, string environment, int pollInterval, int retryNumber, IEnumerable<string> sasl_Brokers, bool sasl_Enabled, string sasl_UserName, string sasl_Password, string dependencyName, int producerRetryCount, int producerRetryInterval, int messageTimeoutMs, int consumerRetryCount, int consumerRetryInterval, string consumerInitialState, int workerCount, int bufferSize, KafkaBatchSettings batch)
        {
            Brokers = brokers;
            Environment = environment;
            PollInterval = pollInterval;
            RetryNumber = retryNumber;
            Sasl_Brokers = sasl_Brokers;
            Sasl_Enabled = sasl_Enabled;
            Sasl_UserName = sasl_UserName;
            Sasl_Password = sasl_Password;
            DependencyName = dependencyName;
            ProducerRetryCount = producerRetryCount;
            ProducerRetryInterval = producerRetryInterval;
            MessageTimeoutMs = messageTimeoutMs;
            ConsumerRetryCount = consumerRetryCount;
            ConsumerRetryInterval = consumerRetryInterval;
            ConsumerInitialState = consumerInitialState;
            WorkerCount = workerCount;
            BufferSize = bufferSize;
            Batch = batch;
        }

        public string Brokers { get; set; }
        public string Environment { get; set; }
        public int PollInterval { get; set; }
        public int RetryNumber { get; set; }
        public IEnumerable<string> Sasl_Brokers { get; set; }
        public bool Sasl_Enabled { get; set; }
        public string Sasl_UserName { get; set; }
        public string Sasl_Password { get; set; }
        public string DependencyName { get; set; }
        public int ProducerRetryCount { get; set; }
        public int ProducerRetryInterval { get; set; }
        public int MessageTimeoutMs { get; set; }
        public int ConsumerRetryCount { get; set; }
        public int ConsumerRetryInterval { get; set; }
        public string ConsumerInitialState { get; set; }
        public int WorkerCount { get; set; }
        public int BufferSize { get; set; }
        public KafkaBatchSettings Batch { get; set; }
    }
}
