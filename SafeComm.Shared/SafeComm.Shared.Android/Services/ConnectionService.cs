using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SafeComm.Shared.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeComm.Shared.Droid.Services.ConnectionService))]
namespace SafeComm.Shared.Droid.Services
{
    public class ConnectionService : IConnectionService
    {
        public Task<string> GetExternalIPAddressAsync()
        {
            throw new NotImplementedException();
        }

        public string GetIPAddress()
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName());
            if (addresses?[0] != null)
                return addresses[0].ToString();
            else
                return null;
        }
    }
}