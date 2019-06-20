using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using SafeComm.Shared.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeComm.Shared.iOS.Services.ConnectionService))]
namespace SafeComm.Shared.iOS.Services
{
    public class ConnectionService : IConnectionService
    {
        public Task<string> GetExternalIPAddressAsync()
        {
            throw new NotImplementedException();
        }

        public string GetIPAddress()
        {
            string ipAddress = string.Empty;
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            ipAddress = addressInfo.Address.ToString();
                    }
                }
            }
            return ipAddress;
        }
    }
}