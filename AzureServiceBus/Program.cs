using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus
{
    class Program
    {

        private static IQueueClient serviceClient;
        private const string connection_string = "Endpoint=sb://systems.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=g02W1R8Vwd435GKxEX3313qIU/xsYrjqHhySs/HhYf0=";
        private const string queue_name = "test_queue";

        public static async Task Main(string[] args)
        {
            serviceClient = new QueueClient(connection_string, queue_name);
            while (true)
            {
                Console.WriteLine("Enter a Message or -1 to Exit Application.");

                string message = Console.ReadLine();

                if (!message.Equals("-1"))
                {
                    await SendAsyncMessage(PutToJSON(message));
                }
                else
                {
                    Console.WriteLine("Exiting");
                    await serviceClient.CloseAsync();
                    break;
                }
            }
            
        }

        private static async Task SendAsyncMessage(string JSON_message)
        {
            try
            {
                Message encoded_message = new Message(Encoding.UTF8.GetBytes(JSON_message));
                Console.WriteLine($"Sending Message: {JSON_message}");
                await serviceClient.SendAsync(encoded_message);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private static string PutToJSON(string message)
        {
            return $"{{ message: \"{message}\" }}";
        }
    }
}
