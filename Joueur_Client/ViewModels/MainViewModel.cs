using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Joueur_Client.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        private BaseViewModel _currentPage;

        public BaseViewModel CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public MainViewModel()
        {
            _currentPage = new MainMenuViewModel();
        }
    }
}
