using SpendManagement.Infra.CrossCutting.Conf;

namespace SpendManagement.API.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Settings GetApplicationSettings(this IConfiguration configuration, IHostEnvironment env)
        {
            var settings = configuration.GetSection("Settings").Get<Settings>();

            if (!env.IsDevelopment())
            {
                settings!.KafkaSettings!.Sasl_Brokers = new[] { GetEnvironmentVariableFromRender("Sasl_Brokers") };
                settings!.KafkaSettings!.Sasl_Username = GetEnvironmentVariableFromRender("Sasl_Username");
                settings!.KafkaSettings!.Sasl_Password = GetEnvironmentVariableFromRender("Sasl_Password");
            }

            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? "";
        }
    }
}
