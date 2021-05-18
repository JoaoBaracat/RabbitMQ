using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using RabbitMQ.Client;

namespace Client
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample1";
        private const string ExchangeName = "";
        

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
                    var myMessage = new MyMessage()
                        {
                            Message = string.Format("Message: {0}", messageCount)
                        };
                    
                    Console.WriteLine(string.Format("Sending - {0}", myMessage.Message));
                    //Setup properties
                    var properties = model.CreateBasicProperties();
                    properties.SetPersistent(true);

                    //Serialize
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(myMessage);
                    byte[] messageBuffer = Encoding.Default.GetBytes(jsonString);

                    //Send message
                    model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
                    messageCount++;
                }
            }
            
            Console.ReadLine();
        }
    }
}
