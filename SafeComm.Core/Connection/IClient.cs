using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Core.Connection
{
    public interface IClient
    {
        void Connect(IPAddress ipAddress, int port);

        void Disconnect();

        void SendMessage (string message);

        Task SendMessageAsync(string message);

        string ReceiveMessage();

        Task<string> ReceiveMessageAsync();
    }
}
