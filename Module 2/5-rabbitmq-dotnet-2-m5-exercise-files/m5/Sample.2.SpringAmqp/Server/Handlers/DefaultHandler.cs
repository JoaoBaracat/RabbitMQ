using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Handlers
{
    public class DefaultHandler
    {
        #region Logging Definition

        private readonly ILog log = LogManager.GetLogger(typeof(DefaultHandler));

        #endregion

        public Contract.WeatherResponse Handle(Contract.WeatherRequest request)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Recieved weather request message");

            return new Contract.WeatherResponse()
            {
                Location = request.Location,
                Temperature = 1,
                WeatherDescription = "Nice"
            };
        }

        public Contract.TimeResponse Handle(Contract.TimeRequest request)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Recieved time request message");

            return new Contract.TimeResponse()
            {
                Time = DateTime.Now
            };
        }
    }
}
