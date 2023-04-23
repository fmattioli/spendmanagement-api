namespace SpendManagement.Infra.CrossCutting.Conf
{
    public record MongoSettings
    {
        public string Database { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
