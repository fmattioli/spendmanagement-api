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
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, Settings? settings)
        {
            var configKafka = new ProducerConfig { BootstrapServers = settings?.KafkaSettings?.Broker};

            if (settings?.SpendManagementDomain is not null && settings.SpendManagementIdentity is not null)
            {
                services
                    .AddHealthChecks()
                    .AddKafka(configKafka, name: "Kafka")
                    .AddUrlGroup(new Uri(settings.SpendManagementDomain.Url! + UrlHealthCheck), name: "SpendManagement.Domain")
                    .AddUrlGroup(new Uri(settings.SpendManagementIdentity.Url + UrlHealthCheck), name: "SpendManagement.Identity");

                services
                    .AddHealthChecksUI(setupSettings: setup => setup.SetEvaluationTimeInSeconds(60))
                    .AddInMemoryStorage();
            }

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
