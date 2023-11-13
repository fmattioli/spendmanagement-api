namespace SpendManagement.Integration.Tests.Configuration
{
    public record MongoSettings
    {
        public string Database { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
