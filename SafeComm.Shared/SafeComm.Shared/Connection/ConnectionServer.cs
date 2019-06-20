using SafeComm.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeComm.Shared.Connection
{
    public class ConnectionServer : IConnectionServer
    {
        private readonly IConnectionService connectionService;
        private TcpListener listener;
        private Socket socket;

        public bool IsConnected { get { return socket.Connected; } }

        public ConnectionServer()
        {
            connectionService = DependencyService.Get<IConnectionService>();
        }

        public void StartListening(IPAddress ipAddress, int port)
        {
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            socket = listener.AcceptSocket();
        }

        public async Task StartListeningAsync(IPAddress ipAddress, int port)
        {
            listener = new TcpListener(ipAddress, port);
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
    }
}
