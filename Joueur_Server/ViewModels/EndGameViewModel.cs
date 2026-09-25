using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class EndGameViewModel : BaseViewModel
    {
        private readonly Server _server;

        public Data LastVersionData { get; set; }

        public System.Action NavigateToConfiguration { get; }
        public System.Action NavigateToMainMenu { get; }


        public EndGameViewModel(Server server, Data data, System.Action navigateToConfiguration, System.Action navigateToMainMenu)
        {
            _server = server;
            LastVersionData = data;
            NavigateToConfiguration = navigateToConfiguration;
            NavigateToMainMenu = navigateToMainMenu;

            _ = ListenToClientChoice();
        }

        private async Task ListenToClientChoice()
        {
            LastVersionData = await _server.ReceiveData();

            if (LastVersionData.PlayAgain)
                NavigateToConfiguration();
            if (!LastVersionData.PlayAgain)
            {
                _server.CloseConnection();
                NavigateToMainMenu();
            }
        }
    }
}
