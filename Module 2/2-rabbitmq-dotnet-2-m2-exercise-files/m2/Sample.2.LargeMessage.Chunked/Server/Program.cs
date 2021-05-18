using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
        private const string QueueName = "Module3.Sample6";
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
                Console.WriteLine("Received message");

                //Get Output File Path
                var pathProperty = (byte[])deliveryArgs.BasicProperties.Headers["OutputFileName"];
                var outputPath = Encoding.Default.GetString(pathProperty);
                var sequenceNumber = (int)deliveryArgs.BasicProperties.Headers["SequenceNumber"];
                

                //Adding message                
                using (var fileStream = new FileStream(outputPath, FileMode.Append, FileAccess.Write))
                {                                        
                    fileStream.Write(deliveryArgs.Body, 0, deliveryArgs.Body.Length);
                    fileStream.Flush();
                }
                Console.WriteLine("Message saved to disk - Sequence No = {0}", sequenceNumber);                

                model.BasicAck(deliveryArgs.DeliveryTag, false);
                Console.WriteLine("Listening for another message");
            }
        }
    }
}
