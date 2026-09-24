using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        private readonly Server _server;
        private readonly Robot _robotServer;
        private readonly Robot _robotClient;

        public GameViewModel(Server server, Robot robotServer, Robot robotClient)
        {
            _server = server;
            _robotServer = robotServer;
            _robotClient = robotClient;
        }
    }
}
