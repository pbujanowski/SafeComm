using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SafeComm.Core.Connection;
using SafeComm.Core.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SafeComm.Test.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Client client;
        private readonly Server server;
        private DiffieHellman diffieHellmanServer;
        private DiffieHellman diffieHellmanClient;

        private string host;
        private int port;
        private string output;
        private string input;

        private string encryptionKey;

        public string Host
        {
            get { return host; }
            set { Set(() => Host, ref host, value); }
        }

        public int Port
        {
            get { return port; }
            set { Set(() => Port, ref port, value); }
        }

        public string Output
        {
            get { return output; }
            set { Set(() => Output, ref output, value); }
        }

        public string Input
        {
            get { return input; }
            set { Set(() => Input, ref input, value); }
        }

        public ICommand ConnectToPeer { get; set; }

        private async void ExecuteConnectToPeer()
        {
            try
            {
                Log("Łączenie z użytkownikiem...");

                client.Connect(IPAddress.Parse(Host), Port);

                Log("Połączono!");
                Log("Wymiana informacji (handshaking)...");

                diffieHellmanServer = new DiffieHellman(128).GenerateRequest();
                await client.SendMessageAsync(diffieHellmanServer.ToString()).ConfigureAwait(false);
                string response = await client.ReceiveMessageAsync().ConfigureAwait(false);

                Log($"Otrzymano odpowiedź: {response}");

                diffieHellmanServer.HandleResponse(response);
                encryptionKey = Convert.ToBase64String(diffieHellmanServer.Key);

                Log($"Uzyskano klucz: {encryptionKey}");

                while (true)
                {
                    string message = await client.ReceiveMessageAsync().ConfigureAwait(false);
                    string decryptedMessage = Rijndael.Decrypt(message, encryptionKey);
                    Log(decryptedMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand SendMessage { get; set; }

        private void ExecuteSendMessage()
        {
            try
            {
                Log(Input);
                string encryptedMessage = Rijndael.Encrypt(Input, encryptionKey);

                if (client.IsConnected)
                    client.SendMessage(encryptedMessage);
                else if (server.IsConnected)
                    server.SendMessage(encryptedMessage);

                Input = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand StartListening { get; set; }

        private async void ExecuteStartListening()
        {
            try
            {
                Log("Nasłuchiwanie...");

                await server.StartListeningAsync().ConfigureAwait(false);

                Log("Połączono!");
                Log("Wymiana informacji (handshaking)...");

                string request = await server.ReceiveMessageAsync().ConfigureAwait(false);

                Log($"Otrzymano żądanie: {request}");

                diffieHellmanClient = new DiffieHellman(128).GenerateResponse(request);
                await server.SendMessageAsync(diffieHellmanClient.ToString()).ConfigureAwait(false);
                encryptionKey = Convert.ToBase64String(diffieHellmanClient.Key);

                Log($"Uzyskano klucz: {encryptionKey}");

                while (true)
                {
                    string message = await server.ReceiveMessageAsync().ConfigureAwait(false);
                    string decryptedMessage = Rijndael.Decrypt(message, encryptionKey);
                    Log(decryptedMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public MainViewModel()
        {
            client = new Client();
            server = new Server();

            ConnectToPeer = new RelayCommand(ExecuteConnectToPeer);
            SendMessage = new RelayCommand(ExecuteSendMessage);
            StartListening = new RelayCommand(ExecuteStartListening);

            Host = "127.0.0.1";
            Port = 1234;
        }

        private void Log(string log)
        {
            Output += $"{DateTime.Now} {DateTime.Now.ToShortTimeString()}: {log} {Environment.NewLine}";
        }
    }
}
