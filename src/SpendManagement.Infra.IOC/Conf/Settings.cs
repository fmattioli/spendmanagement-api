namespace SpendManagement.Infra.CrossCutting.Conf
{
    public interface ISettings
    {
        public KafkaSettings? KafkaSettings { get;}
        public MongoSettings? MongoSettings { get;}
        public SpendManagementReadModel SpendManagementReadModel { get;}
    }

    public record Settings : ISettings
    {
        public SpendManagementReadModel SpendManagementReadModel { get; set; } = null!;
        public KafkaSettings KafkaSettings { get; set; } = null!;
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
