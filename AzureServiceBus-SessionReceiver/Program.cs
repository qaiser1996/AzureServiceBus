﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus_SessionReceiver
{
    class Program
    {

        private const string connection_string = "<connection_string>";
        private const string queue_name = "<queue_name>";
        private static ISessionClient messageSession;

        static async Task Main(string[] args)
        {
            messageSession = new SessionClient(connection_string, queue_name, ReceiveMode.ReceiveAndDelete);

            while (true)
            {
                Console.WriteLine("Enter Session ID or -1 to exit.");
                string sID = Console.ReadLine();

                if (!sID.Equals("-1"))
                {
                    IMessageSession session = await messageSession.AcceptMessageSessionAsync(sID);
                    Message message = await session.ReceiveAsync(TimeSpan.FromSeconds(5));
                    Console.WriteLine($"Message: {Encoding.UTF8.GetString(message.Body)}\nSession: {message.SessionId}");
                    await session.CloseAsync();
                }
                else
                {
                    await messageSession.CloseAsync();
                    break;
                }
            }

            

            
        }
    }
}
