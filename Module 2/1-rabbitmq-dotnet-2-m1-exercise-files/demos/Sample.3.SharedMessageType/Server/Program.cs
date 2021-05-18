using Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample3";
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

                //Get Message Format
                var messageFormat = GetMessageFormat(deliveryArgs.BasicProperties.ContentType);
                Console.WriteLine("Message Content Type = {0}", deliveryArgs.BasicProperties.ContentType);

                //Message Type
                var messageType = deliveryArgs.BasicProperties.Type;
                Console.WriteLine("Message Type = {0}", messageType);

                //Get Message Body as String
                var messageAsString = GetMessageAsString(deliveryArgs.Body, messageFormat);
                Console.WriteLine("Message as string = {0}", messageAsString);

                //Get message and Deserialize
                var myUntypedMessage = DeserializeMessage(deliveryArgs.Body, messageFormat, messageType);

                //Display
                if (myUntypedMessage.GetType() == typeof(MyMessage))
                {
                    var myMessage = myUntypedMessage as MyMessage;
                    Console.WriteLine("Data from object: MyMessage = {0}", myMessage.Message);
                }
                else if (myUntypedMessage.GetType() == typeof(MyMessage2))
                {
                    var myMessage2 = myUntypedMessage as MyMessage2;
                    Console.WriteLine("Data from object: MyMessage2 = {0}", myMessage2.Message);
                }
                else
                {
                    Console.WriteLine("Data from object: Unknown data type");
                }                                

                //Acknowledge
                model.BasicAck(deliveryArgs.DeliveryTag, false);
                Console.WriteLine("Listening for another message");
                Console.WriteLine();
            }
        }


        private static MessageFormat GetMessageFormat(string contentType)
        {
            if (contentType == "application/json")
            {
                return MessageFormat.Json;
            }
            else if (contentType == "text/xml")
            {
                return MessageFormat.Xml;
            }
            else if (contentType == "application/octet-stream")
            {
                return MessageFormat.Binary;
            }
            else
                return MessageFormat.None;
        }

        private static string GetMessageAsString(byte[] body, MessageFormat messageFormat)
        {
            if (messageFormat == MessageFormat.Json)
            {
                return Encoding.Default.GetString(body);
            }
            else if (messageFormat == MessageFormat.Xml)
            {
                return Encoding.Default.GetString(body);
            }
            else if (messageFormat == MessageFormat.Binary)
            {
                return Convert.ToBase64String(body);
            }
            else
                return string.Empty; ;
        }

        private static object DeserializeMessage(byte[] body, MessageFormat messageFormat, string messageType)
        {
            var type = Type.GetType(messageType);

            if (messageFormat == MessageFormat.Json)
            {
                
                var jsonString = Encoding.Default.GetString(body);
                return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, type);                
            }
            else if (messageFormat == MessageFormat.Xml)
            {
                var messageStream = new MemoryStream();
                messageStream.Write(body, 0, body.Length);
                messageStream.Seek(0, SeekOrigin.Begin);
                var xmlSerializer = new XmlSerializer(type);
                return xmlSerializer.Deserialize(messageStream);                
            }
            else if (messageFormat == MessageFormat.Binary)
            {
                var messageStream = new MemoryStream();
                messageStream.Write(body, 0, body.Length);
                messageStream.Seek(0, SeekOrigin.Begin);
                var binarySerializer = new BinaryFormatter();
                return binarySerializer.Deserialize(messageStream);                
            }
            else
                return null;
        }

        private enum MessageFormat
        {
            None,
            Xml,
            Json,
            Binary
        }
    }
}
