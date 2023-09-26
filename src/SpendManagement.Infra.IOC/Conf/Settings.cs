namespace SpendManagement.Infra.CrossCutting.Conf
{
    public interface ISettings
    {
        public string? TokenAuth { get; }
        public TracingSettings? TracingSettings { get; }
        public KafkaSettings? KafkaSettings { get;}
        public MongoSettings? MongoSettings { get;}
        public SpendManagementIdentitySettings? SpendManagementIdentity { get;}
        public SpendManagementReadModelSettings? SpendManagementReadModel { get;}
    }

    public record Settings : ISettings
    {
        public string? TokenAuth { get; set; }
        public TracingSettings? TracingSettings { get; set; }
        public SpendManagementIdentitySettings? SpendManagementIdentity { get; set; }
        public SpendManagementReadModelSettings? SpendManagementReadModel { get; set; }
        public KafkaSettings? KafkaSettings { get; set; }
        public MongoSettings? MongoSettings { get; set; }
    }
}
