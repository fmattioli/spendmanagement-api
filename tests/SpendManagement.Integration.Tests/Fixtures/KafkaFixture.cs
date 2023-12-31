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
                       .WithBrokers(settings!.Brokers)
                       .AddConsumer(consumer =>
                       {
                           consumer
                               .Topic(KafkaTopics.Commands.GetReceiptCommands(settings!.Environment))
                               .WithGroupId("Receipts-Commands")
                               .WithName("Receipt-Commands")
                               .WithBufferSize(2)
                               .WithWorkersCount(4)
                               .WithAutoOffsetReset(AutoOffsetReset.Latest)
                               .AddMiddlewares(middlewares => middlewares
                                   .AddDeserializer<JsonCoreDeserializer>()
                                   .Add(_ => this.kafkaMessage));
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
            await Task.Delay(TestSettings.Kafka!.InitializationDelay);
        }

        public TMessage Consume<TMessage>(Func<TMessage, IMessageHeaders, bool> predicate)
            where TMessage : class
        {
            return this.kafkaMessage.TryTake(
                predicate,
                300 * 200);
        }
    }
}
