using Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample1";
        private const string ExchangeName = "";

        static void Main(string[] args)
        {
            #region Connect to Rabbit
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };
            
            var connection =connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            #endregion

            model.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(QueueName, false, consumer);

            Console.WriteLine("Started and listening for a message");
            while (true)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Get message and Deserialize
                var jsonString = Encoding.Default.GetString(deliveryArgs.Body);
                var myMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<MyMessage>(jsonString);

                Console.WriteLine("Message Recieved: JSON = {0}", jsonString);
                Console.WriteLine("Message Recieved: Message = {0}", myMessage.Message);

                model.BasicAck(deliveryArgs.DeliveryTag, false);
                Console.WriteLine("Listening for another message");
            }
        }
    }
}
