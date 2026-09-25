using Joueur_Client.Models;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Joueur_Client.ViewModels
{
    internal class EndGameViewModel : BaseViewModel
    {
        private readonly Client _client;

        public Data LastVersionData { get; set; }

        public EndGameViewModel(Client client, Data data)
        {
            _client = client;

            LastVersionData = data;
        }
    }
}
