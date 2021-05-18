using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.IO;

namespace Client
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample5";
        private const string ExchangeName = "";

        private const string InputFile = "BigFile.txt";
        

        private static void Main(string[] args)
        {
            #region Connect to Rabbit
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };            

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            #endregion

            var messageCount = 0;

            Console.WriteLine("Press enter key to send a message");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                    break;

                if (key.Key == ConsoleKey.Enter)
                {                                                           
                    //Setup properties
                    var properties = model.CreateBasicProperties();
                    properties.SetPersistent(true);
                    
                    //Read File
                    Console.WriteLine("Reading file - {0}", InputFile);
                    byte[] messageBuffer = File.ReadAllBytes(InputFile);

                    //Send message
                    Console.WriteLine("Sending large message - {0}", messageBuffer.Length);
                    model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
                    messageCount++;
                    Console.WriteLine("Message sent");
                }
            }
            
            Console.ReadLine();
        }
    }
}
