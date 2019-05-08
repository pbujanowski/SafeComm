using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Core.Connection
{
    public interface IServer
    {
        void StartListening();

        Task StartListeningAsync();

        void StopListening();

        void SendMessage(string message);

        Task SendMessageAsync(string message);

        string ReceiveMessage();

        Task<string> ReceiveMessageAsync();
    }
}
