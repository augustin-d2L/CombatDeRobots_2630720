using Joueur_Client.Models;
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
            CurrentPage = new ConfigurationViewModel(_client, NavigateToGame);
        }

        private void NavigateToGame(Robot robotServer, Robot robotClient)
        {
            CurrentPage = new GameViewModel(_client, robotServer, robotClient, NavigateToEndGame);
        }

        private void NavigateToEndGame(Data data)
        {
            CurrentPage = new EndGameViewModel(_client, data, NavigateToConfiguration, NavigateToMainMenu);
        }

        private void NavigateToMainMenu()
        {
            CurrentPage = new MainMenuViewModel(_client, NavigateToConfiguration);
        }
    }
}
