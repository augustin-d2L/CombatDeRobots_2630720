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

        public System.Action NavigateToConfiguration { get; }
        public System.Action<string> NavigateToMainMenu { get; }


        public EndGameViewModel(Server server, Data data, System.Action navigateToConfiguration, System.Action<string> navigateToMainMenu)
        {
            _server = server;
            LastVersionData = data;
            NavigateToConfiguration = navigateToConfiguration;
            NavigateToMainMenu = navigateToMainMenu;

            _ = ListenToClientChoice();
        }

        private async Task ListenToClientChoice()
        {
            try
            {
                LastVersionData = await _server.ReceiveData();
                if (LastVersionData.PlayAgain)
                    NavigateToConfiguration();
                if (!LastVersionData.PlayAgain)
                {
                    _server.CloseConnection();
                    NavigateToMainMenu("Le client à quitté la partie");
                }
            }
            catch (SocketException)
            {
                NavigateToMainMenu("Connexion perdue avec le client");
            }
        }
    }
}
