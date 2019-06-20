using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Shared.Connection
{
    public interface IConnectionServer
    {
        bool IsConnected { get; }

        void StartListening(IPAddress ipAddress, int port);

        Task StartListeningAsync(IPAddress ipAddress, int port);

        void StopListening();

        void SendMessage(string message);

        Task SendMessageAsync(string message);

        string ReceiveMessage();

        Task<string> ReceiveMessageAsync();
    }
}
