using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Joueur_Client.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly Client _client;
        private BaseViewModel _currentPage;

        public BaseViewModel CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public MainViewModel()
        {
            _client = new Client();

            _currentPage = new MainMenuViewModel(_client, NavigateToConfiguration);
        }

        private void NavigateToConfiguration()
        {
            CurrentPage = new ConfigurationViewModel(_client);
        }
    }
}
