using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Admin.Dashboard;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Application.Producers;
using SpendManagement.Contracts.V1.Interfaces;
using SpendManagement.Infra.CrossCutting.Conf;
using SpendManagement.Infra.CrossCutting.Middlewares;
using SpendManagement.Topics;

namespace SpendManagement.Infra.CrossCutting.Extensions.Kafka
{
    public static class KafkaExtension
    {
        public static IApplicationBuilder ShowKafkaDashboard(this IApplicationBuilder app) => app.UseKafkaFlowDashboard();

        public static IServiceCollection AddKafka(this IServiceCollection services, KafkaSettings kafkaSettings)
        {
            services.AddKafka(
                k => k
                    .UseConsoleLog()
                    .AddCluster(
                        cluster => cluster
                        .AddBrokers(kafkaSettings)
                        .AddProducers(kafkaSettings)
                        ));

            services.AddSingleton<ICommandProducer, CommandProducer>();

            return services;
        }

        private static IClusterConfigurationBuilder AddBrokers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings settings)
        {
            if (settings.Sasl_Enabled)
            {
                builder
                    .WithBrokers(settings.Sasl_Brokers)
                    .WithSecurityInformation(si =>
                    {
                        si.SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol.Plaintext;
                        si.SaslUsername = settings.Sasl_UserName;
                        si.SaslPassword = settings.Sasl_Password;
                        si.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.Plain;
                        si.SslCaLocation = string.Empty;
                    });
            }
            else
            {
                builder.WithBrokers(new[] { settings.Broker });
            }

            return builder;
        }

        private static IClusterConfigurationBuilder AddProducers(
           this IClusterConfigurationBuilder builder,
           KafkaSettings settings)
        {
            var producerConfig = new ProducerConfig
            {
                MessageTimeoutMs = settings.MessageTimeoutMs,
            };

            builder
                .CreateTopicIfNotExists(KafkaTopics.Commands.ReceiptCommandTopicName, 2, 1)
                .AddProducer<ICommand>(
                    p => p
                        .DefaultTopic(KafkaTopics.Commands.ReceiptCommandTopicName)
                        .AddMiddlewares(
                            m => m
                                .Add<ProducerTracingMiddleware>()
                                .Add<ProducerRetryMiddleware>()
                                .AddSerializer<JsonCoreSerializer>())
                        .WithAcks(KafkaFlow.Acks.All)
                        .WithProducerConfig(producerConfig));

            return builder;
        }
    }
}
