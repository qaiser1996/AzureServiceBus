using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus_Receiver
{
    class Program
    {
        private static IQueueClient serviceClient;
        private const string connection_string = "<connection_string>";
        private const string queue_name = "test_queue";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            serviceClient = new QueueClient(connection_string, queue_name);
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();
            await serviceClient.CloseAsync();
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler);
            messageHandlerOptions.MaxConcurrentCalls = 1;
            messageHandlerOptions.AutoComplete = false;
            

            serviceClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await serviceClient.CompleteAsync(message.SystemProperties.LockToken);
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
