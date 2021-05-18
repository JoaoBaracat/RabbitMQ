using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Core.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Gateways
{
    public class WeatherServiceGateway : RabbitGatewaySupport, IWeatherServiceGateway
    {
        public Contract.WeatherResponse Send(Contract.WeatherRequest request)
        {
            return (Contract.WeatherResponse)RabbitTemplate.ConvertSendAndReceive(request, delegate(Message message)
            {
                message.MessageProperties.CorrelationId = Guid.NewGuid().ToByteArray();
                return message;
            }) ;
        }
    }
}
