using Joueur_Client.Helpers;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Joueur_Client.ViewModels
{
    internal class MainMenuViewModel : BaseViewModel
    {
        private readonly Client _client;

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
            get { return _ipInput; }
            set { _ipInput = value; }
        }

        private string _portInput;

        public string PortInput
        {
            get { return _portInput; }
            set { _portInput = value; }
        }



        public ICommand ConnectToGameCommand { get; }
        public Action NavigateToConfiguration { get; }

        public MainMenuViewModel(Client client, Action navigateToConfiguration)
        {
            _client = client;   
            NavigateToConfiguration = navigateToConfiguration;
            ConnectToGameCommand = new RelayCommand(ConnectToGame, () => !IsConnecting);
        }

        private async void ConnectToGame()
        {
            IPAddress ip = IPAddress.Parse(IpInput);
            int port = int.Parse(PortInput);

            IsConnecting = true;
            if (await _client.SendData(port, ip))
                NavigateToConfiguration();
        }
    }
}
