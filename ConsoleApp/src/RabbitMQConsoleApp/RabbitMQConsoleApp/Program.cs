using RabbitMQ.Client;
using System;

namespace RabbitMQConsoleApp
{
    class Program
    {
        private const string password = "guest";
        private const string userName = "guest";
        private const string hostName = "localhost";


        static void Main(string[] args)
        {

            var connectionFactory = new RabbitMQ.Client.ConnectionFactory() { 
                Password = password,
                UserName = userName,
                HostName = hostName,
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            //model.    methods to comunicate with the queue
            model.QueueDeclare("MyQueueConsoleApp", true, false, false, null);
            Console.WriteLine("Queue created");

            model.ExchangeDeclare("MyExchangeConsoleApp", ExchangeType.Topic);
            Console.WriteLine("Exchange created");

            model.QueueBind("MyQueueConsoleApp", "MyExchangeConsoleApp", "cars");
            Console.WriteLine("Exchange and queue bound");

            Console.WriteLine("Hello World!");
        }
    }
}
