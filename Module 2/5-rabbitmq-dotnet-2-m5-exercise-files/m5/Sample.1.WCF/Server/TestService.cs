using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class TestService : ITestContract
    {        
        public string TestPing(string message)
        {
            Console.WriteLine(string.Format("Test Service Ping: {0}", message));
            return string.Format("Reply from server is: {0}", message);
        }
    }

}
