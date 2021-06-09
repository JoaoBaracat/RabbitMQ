using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.IO;
using System.Collections.Specialized;

namespace Client
{
    class Program
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample6";
        private const string ExchangeName = "";

        private const string InputFile = "BigFile.txt";
        private const int ChunkSize = 4096;
        

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
            
            Console.WriteLine("Press enter key to send a message");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                    break;

                if (key.Key == ConsoleKey.Enter)
                {
                    var outputFileName = string.Format("{0}.txt", Guid.NewGuid().ToString());

                    var fileStream = File.OpenRead(InputFile);
                    var streamReader = new StreamReader(fileStream);
                    int remaining = (int)fileStream.Length;
                    int length = (int)fileStream.Length;
                    var messageCount = 0;
                    bool endOfSeq = false;

                    while (true)
                    {
                        if (remaining <= 0)
                            break;

                        //Read Chunk                        
                        int read = 0;
                        byte[] buffer;
                        if (remaining > ChunkSize)
                        {
                            buffer = new byte[ChunkSize];
                            read = fileStream.Read(buffer, 0, ChunkSize);
                        }
                        else
                        {
                            buffer = new byte[remaining];
                            read = fileStream.Read(buffer, 0, remaining);
                            endOfSeq = true;
                        }


                        //Setup properties
                        var properties = model.CreateBasicProperties();
                        properties.SetPersistent(true);
                        properties.Headers = new ListDictionary();
                        properties.Headers.Add("OutputFileName", outputFileName);
                        properties.Headers.Add("SequenceNumber", messageCount);                        
                        properties.Headers.Add("EndSeq", endOfSeq);                        

                        //Send message
                        Console.WriteLine("Sending chunk message - Index = {0}; Length = {0}", messageCount, read);
                        model.BasicPublish(ExchangeName, QueueName, properties, buffer);

                        messageCount++;
                        remaining = remaining - read;
                    }

                    Console.WriteLine("Completed sending {0} chunks", messageCount);
                }
            }
            
            Console.ReadLine();
        }
    }
}
