using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus_SessionSender
{
    class Program
    {
        private static IQueueClient serviceClient;
        private const string connection_string = "<connection_string>";
        private const string queue_name = "<queue_name>";

        public static async Task Main(string[] args)
        {
            serviceClient = new QueueClient(connection_string, queue_name);
            while (true)
            {
                Console.WriteLine("Enter a Message or -1 to Exit Application.");

                string message = Console.ReadLine();

                Console.WriteLine("Enter a SessionID.");

                string sID = Console.ReadLine();

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

        private static async Task SendAsyncMessage(string JSON_message, string sessionID)
        {
            try
            {
                Message encoded_message = new Message(Encoding.UTF8.GetBytes(JSON_message));
                encoded_message.SessionId = sessionID;
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
