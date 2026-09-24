using Joueur_Client.Models;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Joueur_Client.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        private readonly Client _client;
        private readonly Robot _robotServer;
        private readonly Robot _robotClient;

        public GameViewModel(Client client, Robot robotServer, Robot robotClient)
        {
            _client = client;
            _robotServer = robotServer;
            _robotClient = robotClient;
        }
    }
}
