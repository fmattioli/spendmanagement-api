using KafkaFlow;
using Polly;
using Spents.Infra.CrossCutting.Conf;

namespace Spents.Infra.CrossCutting.Middlewares
{
    public class ProducerRetryMiddleware : IMessageMiddleware
    {
        private readonly int retryCount;
        private readonly TimeSpan retryInterval;
        public ProducerRetryMiddleware(ISettings settings)
        {
            if (settings.KafkaSettings is not null)
            {
                this.retryCount = settings.KafkaSettings.ProducerRetryCount;
                this.retryInterval = TimeSpan.FromSeconds(settings.KafkaSettings.ProducerRetryInterval);
            }
        }

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            var policyResult = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, retryAttempt, __) =>
                    {
                        Console.WriteLine(ex);
                    })
                .ExecuteAndCaptureAsync(() => next(context));
        }
    }
}
