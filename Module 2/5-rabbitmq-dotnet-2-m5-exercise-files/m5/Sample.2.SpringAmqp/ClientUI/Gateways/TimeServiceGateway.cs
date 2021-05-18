using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Core.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Gateways
{
    public class TimeServiceGateway : RabbitGatewaySupport, ITimeServiceGateway
    {
        public Contract.TimeResponse Send(Contract.TimeRequest request)
        {
            return (Contract.TimeResponse)RabbitTemplate.ConvertSendAndReceive(request, delegate(Message message)
            {
                message.MessageProperties.CorrelationId = Guid.NewGuid().ToByteArray();
                return message;
            }) ;
        }
    }
}
