using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Client
{
    class Program
    {        
        private static void Main(string[] args)
        {
            var index = 0;
            Console.WriteLine("Starting Client");
            Console.WriteLine();
            Console.WriteLine();            


            Console.WriteLine("Press enter key to send a message");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                    break;

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("Sending test message");
                    string response = null;
                    var factory = new ChannelFactory<Contract.ITestContract>("TestService");
                    try
                    {
                        var channel = factory.CreateChannel();
                        response = channel.TestPing(index.ToString());
                    }
                    finally
                    {
                        factory.Close();
                    }
                    Console.WriteLine(string.Format("Response: {0}", response));
                    index++;       
                }
            }
            
            Console.ReadLine();
        }
    }
}
