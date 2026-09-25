using Joueur_Server.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Joueur_Server.Service
{
    internal class Server
    {
        private Socket _socket;
        private readonly Socket _listener;

        public GameService Service { get; set; }
        public string LocalIp { get; }
        public int Port { get; }

        public Server()
        {
            LocalIp = GetLocalIpAddress();
            Service = new GameService();

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 0));
            _listener.Listen(1);

            Port = ((IPEndPoint)_listener.LocalEndPoint).Port;
        }

        public async Task SendData()
        {
            // accepte la connexion et la conserve dans le champ de classe
            _socket = await _listener.AcceptAsync();

            const string eom = "<|EOM|>";
            var buffer = new byte[1_024];

            // recois un message
            int received = await _socket.ReceiveAsync(buffer, SocketFlags.None);

            // extrait le message
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            if (response.IndexOf(eom) > -1)
            {
                // encode le message de retour
                string ackMessage = "<|EOM|>";
                byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);

                // envoie le message de retour
                await _socket.SendAsync(echoBytes, SocketFlags.None);
            }
        }

        public async Task SendData(Data? data = null)
        {
            const string eom = "<|EOM|>";

            if ( data != null )
            {
                string dataString = data.ToJson() + eom;
                byte[] echoBytes = Encoding.UTF8.GetBytes(dataString);

                await _socket.SendAsync(echoBytes, SocketFlags.None);
            }
        }

        public async Task<Data> ReceiveData()
        {
            const string eom = "<|EOM|>";
            var buffer = new byte[1_024];
            string accumulatedResponse = "";

            while (true)
            {
                int received = await _socket.ReceiveAsync(buffer, SocketFlags.None);
                accumulatedResponse += Encoding.UTF8.GetString(buffer, 0, received);

                int eomIndex = accumulatedResponse.IndexOf(eom);
                if (eomIndex > -1)
                {
                    string jsonPart = accumulatedResponse.Substring(0, eomIndex);
                    return Data.FromJson(jsonPart);
                }
            }
        }

        private string GetLocalIpAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint?.Address.ToString() ?? "127.0.0.1";
            }
        }
    }
}