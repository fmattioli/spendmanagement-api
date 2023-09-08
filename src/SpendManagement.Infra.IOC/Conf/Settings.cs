namespace SpendManagement.Infra.CrossCutting.Conf
{
    public interface ISettings
    {
        string TokenAuth { get; }
        public KafkaSettings? KafkaSettings { get;}
        public MongoSettings? MongoSettings { get;}
        public SpendManagementIdentitySettings SpendManagementIdentity { get;}
        public SpendManagementReadModelSettings SpendManagementReadModel { get;}
    }

    public record Settings : ISettings
    {
        public string TokenAuth { get; set; } = null!;
        public SpendManagementIdentitySettings SpendManagementIdentity { get; set; } = null!;
        public SpendManagementReadModelSettings SpendManagementReadModel { get; set; } = null!;
        public KafkaSettings KafkaSettings { get; set; } = null!;
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
