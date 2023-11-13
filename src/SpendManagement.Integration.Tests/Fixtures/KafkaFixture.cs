using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Integration.Tests.Configuration;
using SpendManagement.Integration.Tests.Helpers;
using SpendManagement.Topics;

namespace SpendManagement.Integration.Tests.Fixtures
{
    public class KafkaFixture : IAsyncLifetime
    {
        private readonly IKafkaBus _bus;
        private readonly KafkaMessageHelper kafkaMessage = new();

        public KafkaFixture()
        {
            var settings = TestSettings.Kafka;
            var services = new ServiceCollection();
            services.AddKafka(kafka => kafka
               .UseLogHandler<ConsoleLogHandler>()
               .AddCluster(cluster => cluster
                    .WithBrokers(settings.Brokers)
                    .AddConsumer(consumer =>
                    {
                        consumer
                            .Topic(KafkaTopics.Commands.ReceiptCommandTopicName)
                            .WithGroupId("Receipts-Commands")
                            .WithName("Receipt-Commands")
                            .WithBufferSize(1)
                            .WithWorkersCount(1)
                            .WithAutoOffsetReset(AutoOffsetReset.Latest)
                            .AddMiddlewares(
                                middlewares => middlewares
                                    .AddSerializer<JsonCoreSerializer>());
                    })
            ));

            var provider = services.BuildServiceProvider();
            this._bus = provider.CreateKafkaBus();
        }

        public Task DisposeAsync()
        {
            return this._bus.StopAsync();
        }

        public async Task InitializeAsync()
        {
            await this._bus.StartAsync();
            await Task.Delay(TestSettings.Kafka.InitializationDelay);
        }

        public TMessage Consume<TMessage>(Func<TMessage, IMessageHeaders, bool> predicate)
            where TMessage : class
        {
            return this.kafkaMessage.TryTake(
                predicate,
                300 * 100);
        }
    }
}
