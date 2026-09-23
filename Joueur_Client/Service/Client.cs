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

        public async Task<bool> SendData(int port, IPAddress ip)
        {
            Ip = ip;
            Port = port;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            // Établit la connexion TCP vers le serveur (IP + port donnés à la construction)
            await _socket.ConnectAsync(new IPEndPoint(Ip, Port));

            // Prépare le message de confirmation de connexion
            var message = "CONNECTED !<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message); // texte → octets, seul format que le socket accepte

            // Envoie le message au serveur
            await _socket.SendAsync(messageBytes, SocketFlags.None);

            // Attend la réponse du serveur (l'accusé de réception)
            var buffer = new byte[1_024]; // zone mémoire pour recevoir les octets
            int received = await _socket.ReceiveAsync(buffer, SocketFlags.None); // combien d'octets reçus
            string response = Encoding.UTF8.GetString(buffer, 0, received); // octets reçus → texte

            // Si le serveur confirme bien la réception, la connexion est validée
            return response == "<|ACK|>";
        }

        //public async Task SendData(Data? data = null)

        //receiveData
    }
}
