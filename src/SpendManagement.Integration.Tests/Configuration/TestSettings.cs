using Microsoft.Extensions.Configuration;

namespace SpendManagement.Integration.Tests.Configuration
{
    public static class TestSettings
    {
        static TestSettings()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("testsettings.json", false, true)
               .Build();

            Kafka = config.GetSection("Kafka").Get<KafkaSettings>();
            JwtOptions = config.GetSection("JwtOptions").Get<JwtOptions>();
            MongoSettings = config.GetSection("MongoSettings").Get<MongoSettings>();
        }

        public static KafkaSettings Kafka { get; }
        public static JwtOptions JwtOptions { get; }
        public static MongoSettings MongoSettings { get; }
    }
}
