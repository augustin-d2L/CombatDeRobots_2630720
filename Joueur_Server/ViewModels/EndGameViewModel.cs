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

        public EndGameViewModel(Server server, Data data)
        {
            _server = server;

            LastVersionData = data;
        }
    }
}
