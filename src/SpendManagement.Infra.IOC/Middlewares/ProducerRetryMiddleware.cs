using KafkaFlow;
using Polly;
using Serilog;
using SpendManagement.Infra.CrossCutting.Conf;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public class ProducerRetryMiddleware : IMessageMiddleware
    {
        private readonly int retryCount;
        private readonly TimeSpan retryInterval;
        private readonly ILogger _logger;
        public ProducerRetryMiddleware(ISettings settings, ILogger logger)
        {
            if (settings.KafkaSettings is not null)
            {
                this.retryCount = settings.KafkaSettings.ProducerRetryCount;
                this.retryInterval = TimeSpan.FromSeconds(settings.KafkaSettings.ProducerRetryInterval);
            }

            _logger = logger;
        }

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
             var polyResult = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, retryAttempt, __) =>
                    {
                        Console.WriteLine(ex);
                    })
                .ExecuteAndCaptureAsync(() => next(context));

            _logger.Error(polyResult.FinalException.Message);
        }
    }
}
