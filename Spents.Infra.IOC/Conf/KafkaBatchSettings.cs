namespace Spents.Infra.CrossCutting.Conf
{
    public class KafkaBatchSettings
    {
        public KafkaBatchSettings(int workerCount, int bufferSize, int messageTimeoutSec)
        {
            WorkerCount = workerCount;
            BufferSize = bufferSize;
            MessageTimeoutSec = messageTimeoutSec;
        }

        public int WorkerCount { get; set; }
        public int BufferSize { get; set; }
        public int MessageTimeoutSec { get; set; }
    }
}
