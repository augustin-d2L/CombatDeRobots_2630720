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

        public MainViewModel()
        {
            _server = new Server();

            _currentPage = new MainMenuViewModel(_server);
        }

        private void NavigateToConfiguration()
        {
            //TODO
        }
    }
}
