using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Admin.Dashboard;
using KafkaFlow.Serializer;
using Confluent.Kafka;
using Spents.Infra.CrossCutting.Conf;
using Spents.Infra.CrossCutting.Middlewares;
using Spents.Topics;
using Spents.Events.v1;
using Spents.Core.Domain.Entities;
using Spents.Contracts.Documents;

namespace Spents.Infra.CrossCutting.Extensions.Kafka
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
                        si.SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol.SaslSsl;
                        si.SaslUsername = settings.Sasl_UserName;
                        si.SaslPassword = settings.Sasl_Password;
                        si.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.Plain;
                        si.SslCaLocation = string.Empty;
                    });
            }
            else
            {
                builder.WithBrokers(new[] { settings.Brokers });
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

            builder.
                CreateTopicIfNotExists(KafkaTopics.Events.ReceiptEvents, 2, 1)
                .AddProducer<ReceiptEvent<ReceiptEntity>>(p => p
                .DefaultTopic(KafkaTopics.Events.ReceiptEvents)
                .AddMiddlewares(m => m
                    .Add<ProducerRetryMiddleware>()
                    .AddSerializer<JsonCoreSerializer>())
                .WithAcks(KafkaFlow.Acks.All)
                .WithProducerConfig(producerConfig));

            builder.
                CreateTopicIfNotExists(KafkaTopics.Documents.ReceiptDocuments, 2, 1)
                .AddProducer<ReceiptDocument>(p => p
                .DefaultTopic(KafkaTopics.Documents.ReceiptDocuments)
                .AddMiddlewares(m => m
                    .Add<ProducerRetryMiddleware>()
                    .AddSerializer<JsonCoreSerializer>())
                .WithAcks(KafkaFlow.Acks.All)
                .WithProducerConfig(producerConfig));

            return builder;
        }
    }
}
