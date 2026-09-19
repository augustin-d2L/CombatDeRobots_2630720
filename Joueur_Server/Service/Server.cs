using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Joueur_Server.Service
{
    internal class Server
    {
        private readonly Socket _listener;

        public string LocalIp { get; }
        public int Port { get; }

        public Server()
        {
            LocalIp = GetLocalIpAddress();

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 0));
            _listener.Listen(1);

            Port = ((IPEndPoint)_listener.LocalEndPoint).Port;
        }

        public async Task StartListening()
        {
            Socket handler = await _listener.AcceptAsync();

            const string eom = "<|EOM|>";

            while (true)
            {
                var buffer = new byte[1_024];
                int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);

                if (response.IndexOf(eom) > -1)
                {
                    Console.WriteLine($"Message reçu : \"{response.Replace(eom, "")}\"");

                    string ackMessage = "<|ACK|>";
                    byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(echoBytes, SocketFlags.None);

                    Console.WriteLine($"Accusé de réception envoyé : \"{ackMessage}\"");
                    break;
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