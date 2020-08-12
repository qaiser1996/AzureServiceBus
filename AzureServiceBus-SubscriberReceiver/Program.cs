using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus_SubscriberReceiver
{
    class Program
    {

        private static ISubscriptionClient subscriptionClient;
        private const string connection_string = "<Connection String>";
        private const string topic_name = "<Topic>";
        private const string subscriber_name = "<Subscription Name>";

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            subscriptionClient = new SubscriptionClient(connection_string, topic_name, subscriber_name);

            RegisterMessageHandlerOptions();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();

        }

        private static void RegisterMessageHandlerOptions()
        {
            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Message: {Encoding.UTF8.GetString(message.Body)}");
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
