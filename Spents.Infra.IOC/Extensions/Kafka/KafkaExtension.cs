using Microsoft.Extensions.DependencyInjection;
using KafkaFlow;
using KafkaFlow.Configuration;
using Confluent.Kafka;
using Spents.Infra.CrossCutting.Conf;
using Spents.Infra.CrossCutting.Middlewares;
using KafkaFlow.Serializer;

namespace Spents.Infra.CrossCutting.Extensions.Kafka
{
    public static class KafkaExtension
    {
        private const string topicName = "spent";
        private const string producerName = "spent-producer";

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
                        si.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.ScramSha512;
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

            builder.CreateTopicIfNotExists(topicName, 1, 1)
                        .AddProducer(
                            producerName,
                            producer => producer
                         .DefaultTopic(topicName)
                    .AddMiddlewares(m => m
                                .Add<ProducerRetryMiddleware>()
                                .AddSerializer<JsonCoreSerializer>())
                    .WithAcks(KafkaFlow.Acks.All)
                    .WithProducerConfig(producerConfig));

            return builder;
        }
    }
}
