using Confluent.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Infra.CrossCutting.Conf;

namespace SpendManagement.Infra.CrossCutting.Extensions.HealthCheckers
{
    public static class HealthCheckersExtensions
    {
        private const string UrlHealthCheck = "/health";
        public static IServiceCollection AddHealthCheckers(this IServiceCollection services, Settings settings)
        {
            var configKafka = new ProducerConfig { BootstrapServers = settings.KafkaSettings.Broker};
            services.AddHealthChecks()
                .AddKafka(configKafka, name: "Kafka")
                .AddUrlGroup(new Uri(settings.SpendManagementReadModel.Url + UrlHealthCheck), name: "SpendManagement.ReadModel")
                .AddUrlGroup(new Uri(settings.SpendManagementIdentity.Url + UrlHealthCheck), name: "SpendManagement.Identity");

            services.AddHealthChecksUI()
                .AddInMemoryStorage();
            return services;
        }

        public static void UseHealthCheckers(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => options.UIPath = "/monitor");
        }
    }
}
