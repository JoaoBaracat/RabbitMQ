using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class WeatherResponse
    {
        public string Location { get; set; }
        public string WeatherDescription { get; set; }
        public int Temperature { get; set; }
    }
}
