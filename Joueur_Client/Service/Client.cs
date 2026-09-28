using Joueur_Client.Common;
using Joueur_Client.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Joueur_Client.Service
{
    internal class Client
    {
        private Socket _socket;
        public IPAddress Ip { get; set; }
        public int Port { get; set; }

        public Client()
        {
        }

        public async Task SendData(int port, IPAddress ip)
        {
            Ip = ip;
            Port = port;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Établit la connexion TCP vers le serveur (IP + port donnés à la construction)
            await _socket.ConnectAsync(new IPEndPoint(Ip, Port));
        }

        public async Task SendData(Data? data = null)
        {
            if (data != null)
            {
                string dataString = data.ToJson() + GameConstants.EOM_DELIMITER;
                byte[] echoBytes = Encoding.UTF8.GetBytes(dataString);

                await _socket.SendAsync(echoBytes, SocketFlags.None);
            }
        }

        public async Task<Data> ReceiveData()
        {
            var buffer = new byte[1_024];
            string accumulatedResponse = "";

            while (true)
            {
                int received = await _socket.ReceiveAsync(buffer, SocketFlags.None);
                accumulatedResponse += Encoding.UTF8.GetString(buffer, 0, received);

                int eomIndex = accumulatedResponse.IndexOf(GameConstants.EOM_DELIMITER);
                if (eomIndex > -1)
                {
                    string jsonPart = accumulatedResponse.Substring(0, eomIndex);
                    return Data.FromJson(jsonPart);
                }
            }
        }

        public async void CloseConnection()
        {
            _socket.Close();
        }
    }
}
