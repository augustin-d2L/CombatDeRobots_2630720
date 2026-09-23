using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Joueur_Client.ViewModels
{
    internal class ConfigurationViewModel : BaseViewModel
    {
        private readonly Client _client;
        public ConfigurationViewModel(Client client)
        {
            _client = client;
        }
    }
}
