using SafeComm.Shared.Connection;
using SafeComm.Shared.Encryption;
using SafeComm.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeComm.Shared.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        private readonly IConnectionClient connectionClient;
        private readonly IConnectionServer connectionServer;
        private readonly IDialogService dialogService;
        private Rijndael rijndael = new Rijndael();

        private DiffieHellman diffieHellmanServer;
        private DiffieHellman diffieHellmanClient;
        private string encryptionKey;

        private string myIpAddress;
        private string host;
        private int port;
        private string output;
        private string input;

        public string Host
        {
            get { return host; }
            set { SetProperty(ref host, value); }
        }

        public int Port
        {
            get { return port; }
            set { SetProperty(ref port, value); }
        }

        public string Output
        {
            get { return output; }
            set { SetProperty(ref output, value); }
        }

        public string Input
        {
            get { return input; }
            set { SetProperty(ref input, value); }
        }

        public string MyIPAddress
        {
            get { return myIpAddress; }
            set { SetProperty(ref myIpAddress, value); }
        }

        public Command ConnectToPeer { get; set; }

        private async Task ExecuteConnectToPeer()
        {
            try
            {
                Log("Łączenie z użytkownikiem...");

                await Task.Run(() => connectionClient.Connect(IPAddress.Parse(Host), Port)).ConfigureAwait(false);

                Log("Połączono!");
                Log("Wymiana informacji (handshaking)...");

                diffieHellmanServer = new DiffieHellman(128).GenerateRequest();
                await connectionClient.SendMessageAsync(diffieHellmanServer.ToString()).ConfigureAwait(false);
                string response = await connectionClient.ReceiveMessageAsync().ConfigureAwait(false);

                Log($"Otrzymano odpowiedź: {response}");

                diffieHellmanServer.HandleResponse(response);
                encryptionKey = Convert.ToBase64String(diffieHellmanServer.Key);

                Log($"Uzyskano klucz: {encryptionKey}");

                while (true)
                {
                    string message = await connectionClient.ReceiveMessageAsync().ConfigureAwait(false);
                    string decryptedMessage = rijndael.Decrypt(message, encryptionKey);
                    Log(decryptedMessage);
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Błąd!", ex.Message);
                connectionClient.Disconnect();
            }
        }

        public Command SendMessage { get; set; }

        private void ExecuteSendMessage()
        {
            try
            {
                Log(Input);
                string encryptedMessage = rijndael.Encrypt(Input, encryptionKey);

                if (connectionClient.IsConnected)
                {
                    Log($"Zaszyfrowana wiadomosc: {encryptedMessage}");
                    connectionClient.SendMessage(encryptedMessage);
                }

                else if (connectionServer.IsConnected)
                {
                    Log($"Zaszyfrowana wiadomosc: {encryptedMessage}");
                    connectionClient.SendMessage(encryptedMessage);
                }

                Input = string.Empty;
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Błąd!", ex.Message);
            }
        }

        public Command StartListening { get; set; }

        private async Task ExecuteStartListening()
        {
            try
            {
                Log("Nasłuchiwanie...");

                MyIPAddress = "Proszę czekać...";

                MyIPAddress = DependencyService.Get<IConnectionService>().GetIPAddress();
                await connectionServer.StartListeningAsync(IPAddress.Parse(MyIPAddress), 8080).ConfigureAwait(false);

                Log("Połączono!");
                Log("Wymiana informacji (handshaking)...");

                string request = await connectionServer.ReceiveMessageAsync().ConfigureAwait(false);

                Log($"Otrzymano żądanie: {request}");

                diffieHellmanClient = new DiffieHellman(128).GenerateResponse(request);
                await connectionServer.SendMessageAsync(diffieHellmanClient.ToString()).ConfigureAwait(false);
                encryptionKey = Convert.ToBase64String(diffieHellmanClient.Key);

                Log($"Uzyskano klucz: {encryptionKey}");

                while (true)
                {
                    string message = await connectionServer.ReceiveMessageAsync().ConfigureAwait(false);
                    string decryptedMessage = rijndael.Decrypt(message, encryptionKey);
                    Log(decryptedMessage);
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Błąd!", ex.Message);
            }
        }

        public TestViewModel()
        {
            connectionClient = DependencyService.Get<IConnectionClient>();
            connectionServer = DependencyService.Get<IConnectionServer>();
            dialogService = DependencyService.Get<IDialogService>();

            ConnectToPeer = new Command(async () => await ExecuteConnectToPeer().ConfigureAwait(false));
            SendMessage = new Command(ExecuteSendMessage);
            StartListening = new Command(async () => await ExecuteStartListening().ConfigureAwait(false));

            MyIPAddress = "Uruchom nasłuchiwanie...";

            Host = "127.0.0.1";
            Port = 8080;
        }

        private void Log(string log)
        {
            Output += $"{DateTime.Now} {DateTime.Now.ToShortTimeString()}: {log} {Environment.NewLine}";
        }
    }
}
