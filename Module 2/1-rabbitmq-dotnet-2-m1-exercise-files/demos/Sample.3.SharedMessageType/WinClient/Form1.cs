using Contracts;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WinClient
{
    public partial class Form1 : Form
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample3";
        private const string ExchangeName = "";

        private int messageCount;

        public Form1()
        {
            InitializeComponent();
        }

        private void sendButton_Click(object sender, EventArgs e)
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


            var format = messageFormatComboBox.Text;
            if (string.IsNullOrEmpty(format))
            {
                MessageBox.Show("Please select message format", "Message Format");
                return;
            }
            

            var myMessage = new MyMessage()
            {                
                Message = string.Format("Message: {0}", messageCount)
            };

            //Get Properties
            var messageFormat = GetMessageFormat(format);
            var contentType = GetContentType(messageFormat);
            var messageType = GetMessageType(myMessage);

            //Serialize                    
            byte[] messageBuffer = SerializeMessage(myMessage, messageFormat);

            //Setup properties
            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.ContentType = contentType;
            properties.Type = messageType;

            //Send message
            model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
            messageCount++;
              
        }

        private void button1_Click(object sender, EventArgs e)
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


            var format = messageFormatComboBox.Text;
            if (string.IsNullOrEmpty(format))
            {
                MessageBox.Show("Please select message format", "Message Format");
                return;
            }
            
            var myMessage = new MyMessage2()
            {
                Message = string.Format("Message: {0}", messageCount)
            };

            //Get Properties
            var messageFormat = GetMessageFormat(format);
            var contentType = GetContentType(messageFormat);
            var messageType  = GetMessageType(myMessage);

            //Serialize                    
            byte[] messageBuffer = SerializeMessage(myMessage, messageFormat);

            //Setup properties
            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.ContentType = contentType;
            properties.Type = messageType;

            //Send message
            model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
            messageCount++;
        }

        public static string GetMessageType(object message)
        {
            return message.GetType().AssemblyQualifiedName;
        }

        private static string GetContentType(MessageFormat format)
        {
            if (format == MessageFormat.Json)
            {
                return "application/json";
            }
            else if (format == MessageFormat.Xml)
            {
                return "text/xml";
            }
            else if (format == MessageFormat.Binary)
            {
                return "application/octet-stream";
            }
            else
                return string.Empty;
        }

        private static MessageFormat GetMessageFormat(string format)
        {
            format = format.ToLower();
            if (format == "json")
            {
                return MessageFormat.Json;
            }
            else if (format == "xml")
            {
                return MessageFormat.Xml;
            }
            else if (format == "binary")
            {
                return MessageFormat.Binary;
            }
            else
                return MessageFormat.None;
        }

        private static byte[] SerializeMessage(object message, MessageFormat messageFormat)
        {
            if (messageFormat == MessageFormat.Json)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                return Encoding.Default.GetBytes(jsonString);
            }
            else if (messageFormat == MessageFormat.Xml)
            {
                var messageStream = new MemoryStream();
                var xmlSerializer = new XmlSerializer(message.GetType());
                xmlSerializer.Serialize(messageStream, message);
                messageStream.Flush();
                messageStream.Seek(0, SeekOrigin.Begin);
                return messageStream.GetBuffer();
            }
            else if (messageFormat == MessageFormat.Binary)
            {
                var messageStream = new MemoryStream();
                var binarySerializer = new BinaryFormatter();
                binarySerializer.Serialize(messageStream, message);
                messageStream.Flush();
                messageStream.Seek(0, SeekOrigin.Begin);
                return messageStream.GetBuffer();
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
