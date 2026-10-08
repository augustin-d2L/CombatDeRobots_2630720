using Joueur_Client.Helpers;
using Joueur_Client.Models;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Joueur_Client.ViewModels
{
    internal class EndGameViewModel : BaseViewModel
    {
        private readonly Client _client;

        public string Message => LastVersionData.Message;

        private bool _isRestarting;

        public bool IsRestarting
        {
            get => _isRestarting;
            set
            {
                if (SetProperty(ref _isRestarting, value))
                {
                    (RestartGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        private bool _isQuitting;

        public bool IsQuitting
        {
            get => _isQuitting;
            set
            {
                if (SetProperty(ref _isQuitting, value))
                {
                    (QuitGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Data LastVersionData { get; set; }

        public ICommand RestartGameCommand { get; }
        public ICommand QuitGameCommand { get; }

        private readonly Action _navigateToConfiguration;
        private readonly Action<string> _navigateToMainMenu;

        public EndGameViewModel(Client client, Data data, Action navigateToConfiguration, Action<string> navigateToMainMenu)
        {
            _client = client;
            LastVersionData = data;
            _navigateToConfiguration = navigateToConfiguration;
            _navigateToMainMenu = navigateToMainMenu;

            RestartGameCommand = new RelayCommand(RestartGame, () => !IsRestarting);
            QuitGameCommand = new RelayCommand(QuitGame, () => !IsQuitting);
        }

        private async void RestartGame()
        {
            try
            {
                IsRestarting = true;
                LastVersionData.PlayAgain = true;
                await _client.SendData(LastVersionData);
                _navigateToConfiguration();
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le serveur");
            }
        }

        private async void QuitGame()
        {
            try
            {
                IsQuitting = true;
                LastVersionData.PlayAgain = false;
                await _client.SendData(LastVersionData);
                _client.CloseConnection();
                _navigateToMainMenu("");
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le serveur");
            }
        }
    }
}
