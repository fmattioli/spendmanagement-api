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

        public static IServiceCollection AddKafka(this IServiceCollection services, KafkaSettings? kafkaSettings)
        {
            services.AddKafka(
                k => k
                    .UseConsoleLog()
                    .AddCluster(
                        cluster => cluster
                        .WithBrokers(new string[] { "unique-camel-8345-eu2-kafka.upstash.io:9092" })
                        .WithSecurityInformation(information =>
                        {
                            information.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.Plain;
                            information.SaslUsername = "dW5pcXVlLWNhbWVsLTgzNDUk8RLsTQoJ7i1X5nGz0HNWvMirQdh7ldh4--2vvmY";
                            information.SaslPassword = "ZmExNzIwZDgtYTI4ZC00OTFhLWI5YzgtMzMyMzFkYjBiMjEz";
                            information.SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol.SaslSsl;
                            information.EnableSslCertificateVerification = true;
                        })
                        .CreateTopicIfNotExists(KafkaTopics.Commands.GetReceiptCommands(kafkaSettings!.Environment), 2, 1)
                        .AddProducers(kafkaSettings)
                        ));

            services.AddSingleton<ICommandProducer, CommandProducer>();

            return services;
        }

        private static IClusterConfigurationBuilder AddProducers(
           this IClusterConfigurationBuilder builder,
           KafkaSettings? settings)
        {
            var producerConfig = new ProducerConfig
            {
                MessageTimeoutMs = settings?.MessageTimeoutMs
            };

            builder
                .AddProducer<ICommand>(
                    p => p
                        .DefaultTopic(KafkaTopics.Commands.GetReceiptCommands(settings!.Environment))
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
