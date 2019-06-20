using SafeComm.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeComm.Shared.WPF.Services.ConnectionService))]
namespace SafeComm.Shared.WPF.Services
{
    public class ConnectionService : IConnectionService
    {
        public string GetIPAddress()
        {
            var hostEntry = Dns.GetHostEntry(Environment.MachineName);
            foreach (var address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address.ToString();
            }
            return null;
        }
    }
}
