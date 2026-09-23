using Joueur_Server.Helpers;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Joueur_Server.ViewModels
{
    internal class MainMenuViewModel : BaseViewModel
    {
        private readonly Server _server;

        public string LocalIp { get; }
        public int Port { get; }
        private bool _isHosting;
        public bool IsHosting
        {
            get => _isHosting;
            set
            {
                if (SetProperty(ref _isHosting, value))
                {
                    (HostGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand HostGameCommand { get; }
        public Action NavigateToConfiguration { get; }

        public MainMenuViewModel(Server server, Action navigateToConfiguration)
        {
            _server = server;
            NavigateToConfiguration = navigateToConfiguration;
            LocalIp = server.LocalIp;
            Port = server.Port;

            HostGameCommand = new RelayCommand(HostGame, () => !IsHosting);
        }

        private async void HostGame()
        {
            IsHosting = true;
            await _server.SendData();
            NavigateToConfiguration();
        }
    }
}
