using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Shared.Connection
{
    public interface IConnectionClient
    {
        bool IsConnected { get; }

        void Connect(IPAddress ipAddress, int port);

        Task ConnectAsync(IPAddress ipAddress, int port);

        void Disconnect();

        void SendMessage (string message);

        Task SendMessageAsync(string message);

        string ReceiveMessage();

        Task<string> ReceiveMessageAsync();
    }
}
