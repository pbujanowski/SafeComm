using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Core.Connection
{
    public class Client : IClient
    {
        private readonly TcpClient client;
        private NetworkStream stream;

        public bool IsConnected { get { return client.Connected; } }

        public Client()
        {
            client = new TcpClient();
        }

        public async void Connect(IPAddress ipAddress, int port)
        {
            await client.ConnectAsync(ipAddress, port)
                .ConfigureAwait(false);
            stream = client.GetStream();
        }

        public void Disconnect()
        {
            stream?.Dispose();
            client.Dispose();
        }

        public string ReceiveMessage()
        {
            string message = string.Empty;
            byte[] bytes = new byte[100];
            var length = stream.Read(bytes, 0, bytes.Length);

            for (int i = 0; i < length; i++)
                message += Convert.ToChar(bytes[i]);

            return message;
        }

        public async Task<string> ReceiveMessageAsync()
        {
            string message = string.Empty;
            byte[] bytes = new byte[100];
            var length = await stream.ReadAsync(bytes, 0, bytes.Length)
                .ConfigureAwait(false);

            for (int i = 0; i < length; i++)
                message += Convert.ToChar(bytes[i]);

            return message;
        }

        public void SendMessage(string message)
        {
            var bytes = new ASCIIEncoding().GetBytes(message);
            if (stream != null)
                stream.Write(bytes, 0, bytes.Length);
            else
                throw new NullReferenceException("Nie można wysłać wiadomości, ponieważ nie zdefiniowano odbiorcy!");
        }

        public async Task SendMessageAsync(string message)
        {
            var bytes = new ASCIIEncoding().GetBytes(message);
            if (stream != null)
            {
                await stream.WriteAsync(bytes, 0, bytes.Length)
                   .ConfigureAwait(false);
            }
            else
            {
                throw new NullReferenceException("Nie można wysłać wiadomości, ponieważ nie zdefiniowano odbiorcy!");
            }
        }
    }
}
