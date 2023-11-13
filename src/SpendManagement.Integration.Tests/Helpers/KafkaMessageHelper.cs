using KafkaFlow;
using System.Diagnostics;

namespace SpendManagement.Integration.Tests.Helpers
{
    public sealed class KafkaMessageHelper : IMessageMiddleware
    {
        private readonly Dictionary<Type, List<IMessageContext>> container = new();

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            Debug.WriteLine($"[KafkaMessageHelper] - Received '{context.Message.GetType().FullName}' message.");

            lock (this.container)
            {
                this.GetMessagesOfType(context.Message.Value.GetType()).Add(context);

                Monitor.PulseAll(this.container);
            }

            await next(context);
        }

        public TMessage TryTake<TMessage>(Func<TMessage, IMessageHeaders, bool> predicate, int timeout)
            where TMessage : class
        {
            var sw = Stopwatch.StartNew();

            lock (this.container)
            {
                var contexts = this.GetMessagesOfType(typeof(TMessage));

                int index;

                while ((index = contexts.FindIndex(c => predicate((TMessage)c.Message.Value, c.Headers))) < 0)
                {
                    var elapsed = (int)sw.ElapsedMilliseconds;

                    if (elapsed > timeout || !Monitor.Wait(this.container, timeout - elapsed))
                    {
                        return null!;
                    }
                }

                Debug.WriteLine($"[KafkaMessageHelper] - Consumed '{typeof(TMessage).FullName}' message.");

                var context = contexts[index];
                contexts.RemoveAt(index);
                return (TMessage)context.Message.Value;
            }
        }

        private List<IMessageContext> GetMessagesOfType(Type type)
        {
            if (!this.container.TryGetValue(type, out var list))
            {
                list = this.container[type] = new List<IMessageContext>();
            }

            return list;
        }
    }
}
