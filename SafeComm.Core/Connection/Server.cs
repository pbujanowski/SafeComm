using SafeComm.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Core.Connection
{
    public class Server : IServer
    {
        private readonly IPAddress ipAddress;
        private readonly TcpListener listener;
        private Socket socket;

        public bool IsConnected { get { return socket.Connected; } }

        public void StartListening()
        {
            listener.Start();
            socket = listener.AcceptSocket();
        }

        public async Task StartListeningAsync()
        {
            listener.Start();
            socket = await listener.AcceptSocketAsync()
                .ConfigureAwait(false);
        }

        public void StopListening()
        {
            socket?.Close();
            listener.Stop();
        }

        public async Task SendMessageAsync(string message)
        {
            await Task.Run(() => socket?.Send(new ASCIIEncoding().GetBytes(message)))
                .ConfigureAwait(false);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            string message = string.Empty;
            byte[] bytes = new byte[100];
            int length = await Task.Run(() => socket.Receive(bytes))
                .ConfigureAwait(false);

            for (int i = 0; i < length; i++)
                message += Convert.ToChar(bytes[i]);

            return message;
        }

        public void SendMessage(string message)
        {
            socket?.Send(new ASCIIEncoding().GetBytes(message));
        }

        public string ReceiveMessage()
        {
            string message = string.Empty;
            byte[] bytes = new byte[100];
            int length = socket.Receive(bytes);

            for (int i = 0; i < length; i++)
                message += Convert.ToChar(bytes[i]);

            return message;
        }

        public Server()
        {
            ipAddress = TcpHelper.GetLocalIPAddress();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
        }
    }
}
