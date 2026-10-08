using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly Server _server;
        private BaseViewModel _currentPage;

        public BaseViewModel CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        private BaseViewModel? _currentPopup;
        public BaseViewModel? CurrentPopup
        {
            get => _currentPopup;
            set => SetProperty(ref _currentPopup, value);
        }

        public MainViewModel()
        {
            _server = new Server();

            CurrentPage = new MainMenuViewModel(_server, NavigateToConfiguration);
        }

        private void NavigateToConfiguration()
        {
            CurrentPage = new ConfigurationViewModel(_server, NavigateToGame, NavigateToMainMenu);
        }

        private void NavigateToGame(Robot robotServer, Robot robotClient)
        {
            CurrentPage = new GameViewModel(_server, robotServer, robotClient, NavigateToEndGame, NavigateToMainMenu, AskQuestion);
        }

        private void NavigateToEndGame(Data data)
        {
            CurrentPage = new EndGameViewModel(_server, data, NavigateToConfiguration, NavigateToMainMenu);
        }

        private void NavigateToMainMenu(string? msg = null)
        {
            CurrentPage = new MainMenuViewModel(_server, NavigateToConfiguration, msg);
        }

        private Task<int> AskQuestion(string question)
        {
            var tcs = new TaskCompletionSource<int>();

            CurrentPopup = new EquationPopupViewModel(question, answer =>
            {
                CurrentPopup = null;
                tcs.TrySetResult(answer);
            });

            return tcs.Task;
        }
    }
}