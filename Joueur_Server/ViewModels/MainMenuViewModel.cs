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
        public string Message { get; }
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

        private readonly Action _navigateToConfiguration;

        public MainMenuViewModel(Server server, Action navigateToConfiguration, string? message = "")
        {
            _server = server;
            _navigateToConfiguration = navigateToConfiguration;
            LocalIp = server.LocalIp;
            Port = server.Port;
            Message = message;

            HostGameCommand = new RelayCommand(HostGame, () => !IsHosting);
            HostGame();
        }

        private async void HostGame()
        {
            IsHosting = true;
            await _server.SendData();
            _navigateToConfiguration();
        }
    }
}
