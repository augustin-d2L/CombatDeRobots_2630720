using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class ConfigurationViewModel
    {
        private readonly Server _server;
        public ConfigurationViewModel(Server server)
        {
            _server = server;
        }
    }
}
