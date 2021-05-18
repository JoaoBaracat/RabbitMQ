using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Gateways
{
    
    public interface IWeatherServiceGateway
    {
        Contract.WeatherResponse Send(Contract.WeatherRequest request);
    }
}
