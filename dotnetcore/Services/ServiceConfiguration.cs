using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class ServiceConfiguration
    {
        public readonly EmailManager EmailManager = null;

        public ServiceConfiguration()
        {
            EmailManager = new EmailManager();
        }

        public ServiceConfiguration(string host, int port)
        {
            EmailManager = new EmailManager(host, port);
        }

        public void Details(string from,
            string to, string fromPassword)
        {
            EmailManager.To = to;
            EmailManager.From = from;
            EmailManager.SetPassword = fromPassword;
        }
    }
}
