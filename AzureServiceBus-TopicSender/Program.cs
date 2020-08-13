using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus_TopicSender
{
    class Program
    {
        private const string connection_string = "<Connection String>";
        private const string topic_name = "<Topic>";
        private static ITopicClient topicClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine(Guid.NewGuid().ToString());
            topicClient = new TopicClient(connection_string, topic_name);

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
                    await topicClient.CloseAsync();
                    break;
                }
            }
        }

        private static async Task SendAsyncMessage(string JSONmessage)
        {
            try
            {
                Message message = new Message(Encoding.UTF8.GetBytes(JSONmessage));
                Console.WriteLine($"Sending Message: {JSONmessage}");
                await topicClient.SendAsync(message);
            }
            catch (Exception)
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
