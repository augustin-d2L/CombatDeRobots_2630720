using Joueur_Client.Helpers;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Joueur_Client.ViewModels
{
    internal class MainMenuViewModel : BaseViewModel
    {
        private readonly Client _client;

        public string Message { get; }

        private bool _isConnecting;

        public bool IsConnecting
        {
            get => _isConnecting; 
            set 
            {
                if (SetProperty(ref _isConnecting, value))
                {
                    (ConnectToGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _ipInput;
        public string IpInput
        {
            get => _ipInput;
            set => SetProperty(ref _ipInput, value);
        }

        private string _portInput;
        public string PortInput
        {
            get => _portInput;
            set => SetProperty(ref _portInput, value);
        }

        public ICommand ConnectToGameCommand { get; }

        public Action NavigateToConfiguration { get; }

        public MainMenuViewModel(Client client, Action navigateToConfiguration, string? message = "")
        {
            _client = client;
            Message = message;
            NavigateToConfiguration = navigateToConfiguration;
            ConnectToGameCommand = new RelayCommand(ConnectToGame, () => !IsConnecting);
        }

        private async void ConnectToGame()
        {
            try
            {
                bool inputIsCorrect = true;
                if (!IPAddress.TryParse(IpInput, out IPAddress? ip))
                {
                    IpInput = "0.0.0.0";
                    inputIsCorrect = false;
                }

                if (!int.TryParse(PortInput, out int port))
                {
                    PortInput = "0";
                    inputIsCorrect = false;
                }

                if (inputIsCorrect)
                {
                    IsConnecting = true;
                    await _client.SendData(port, ip);
                    NavigateToConfiguration();
                }
            }
            catch (SocketException)
            {
                IsConnecting = false;
            }
        }
    }
}
