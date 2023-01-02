namespace Spents.Infra.CrossCutting.Conf
{
    public interface ISettings
    {
        public KafkaSettings? KafkaSettings { get;}
    }

    public record Settings : ISettings
    {
        public KafkaSettings? KafkaSettings { get; set; }
    }
}
