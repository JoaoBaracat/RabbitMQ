using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Gateways
{
    
    public interface ITimeServiceGateway
    {
        Contract.TimeResponse Send(Contract.TimeRequest request);
    }
}
