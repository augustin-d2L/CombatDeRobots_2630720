using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class EndGameViewModel : BaseViewModel
    {
        private readonly Server _server;

        public string Message => LastVersionData.Message;

        public Data LastVersionData { get; set; }

        private readonly System.Action _navigateToConfiguration;
        private readonly System.Action<string> _navigateToMainMenu;


        public EndGameViewModel(Server server, Data data, System.Action navigateToConfiguration, System.Action<string> navigateToMainMenu)
        {
            _server = server;
            LastVersionData = data;
            _navigateToConfiguration = navigateToConfiguration;
            _navigateToMainMenu = navigateToMainMenu;

            _ = ListenToClientChoice();
        }

        private async Task ListenToClientChoice()
        {
            try
            {
                LastVersionData = await _server.ReceiveData();
                if (LastVersionData.PlayAgain)
                    _navigateToConfiguration();
                if (!LastVersionData.PlayAgain)
                {
                    _server.CloseConnection();
                    _navigateToMainMenu("Le client à quitté la partie");
                }
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le client");
            }
        }
    }
}
