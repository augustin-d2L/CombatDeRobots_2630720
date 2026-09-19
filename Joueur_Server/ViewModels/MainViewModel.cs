using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public Server Server { get; set; }

        private BaseViewModel _currentPage;

        public BaseViewModel CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public MainViewModel()
        {
            Server = new Server();

            _currentPage = new MainMenuViewModel(Server);
        }
    }
}
