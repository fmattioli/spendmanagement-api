using KafkaFlow;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;
using System.Diagnostics;
using System.Text;
using SpendManagement.Application.Constants;
using SpendManagement.Topics;
using SpendManagement.Infra.CrossCutting.Conf;

namespace SpendManagement.Infra.CrossCutting.Middlewares
{
    public class ProducerTracingMiddleware(ISettings settings) : IMessageMiddleware
    {
        private static readonly ActivitySource Activity = new(Constants.ApplicationName);
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
        private readonly string environment = settings!.KafkaSettings!.Environment;

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            using (var activity = Activity.StartActivity("Kafka publish", ActivityKind.Producer))
            {
                if (activity is not null)
                    AddActivityToHeader(activity, context);
            }

            await next(context);
        }

        private void AddActivityToHeader(Activity activity, IMessageContext props)
        {
            Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props, InjectContextIntoHeader);
            activity?.SetTag("messaging.system", "Kafka");
            activity?.SetTag("messaging.destination_kind", "queue");
            activity?.SetTag("messaging..topic", KafkaTopics.Commands.GetReceiptCommands(environment));
        }

        private void InjectContextIntoHeader(IMessageContext props, string key, string value)
        {
            props.Headers.Add(key, Encoding.ASCII.GetBytes(value));
        }
    }
}
