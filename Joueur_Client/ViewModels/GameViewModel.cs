using Joueur_Client.Helpers;
using Joueur_Client.Models;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Joueur_Client.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        private readonly Client _client;
        private readonly Robot _robotServer;
        private readonly Robot _robotClient;

        private bool _isTurnToPlay;

        public bool IsTurnToPlay
        {
            get { return _isTurnToPlay; }
            set
            {
                if (SetProperty(ref _isTurnToPlay, value))
                {
                }
            }
        }
        
        private bool _choseAction;

        public bool ChoseAction
        {
            get { return _choseAction; }
            set
            {
                if (SetProperty(ref _choseAction, value))
                {
                    (PlayTurnCommand as RelayCommand)?.RaiseCanExecuteChanged();// j'essaie de dire que quand une action est choisie le bouton se grise
                }
            }
        }

        private string? _selectedAction;
        public string? SelectedAction
        {
            get => _selectedAction;
            private set
            {
                if (SetProperty(ref _selectedAction, value))
                {
                    (PlayTurnCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Robot du client
        public string RobotName => _robotClient.Name;
        public int HealthPoints => _robotClient.HealthPoints;
        public int Armor => _robotClient.Armor;
        public int Damage => _robotClient.Damage;

        // Robot du server
        public string EnnemyRobotName => _robotServer.Name;
        public int EnnemyHealthPoints => _robotServer.HealthPoints;
        public int EnnemyArmor => _robotServer.Armor;
        public int EnnemyDamage => _robotServer.Damage;

        public ICommand PlayTurnCommand { get; }
        public ICommand SelectActionCommand { get; }

        public GameViewModel(Client client, Robot robotServer, Robot robotClient)
        {
            _client = client;
            _robotServer = robotServer;
            _robotClient = robotClient;

            IsTurnToPlay = true;

            SelectActionCommand = new RelayCommand(SelectAction);
            PlayTurnCommand = new RelayCommand(PlayTurn, CanPlayTurn);
        }

        private bool CanPlayTurn(object? _) => IsTurnToPlay && SelectedAction != null && !ChoseAction;
        private async void PlayTurn(object? _)
        {
            //TODO effectuer l'action
            //Envoyer à l'autre l'action
            //UpdateDisplay();
            //Recevoir l'action 
            //UpdateDisplay();
        }

        private void SelectAction(object? actionName)
        {
            SelectedAction = actionName as string;
        }

        private void UpdateDisplay()
        {
            OnPropertyChanged(nameof(HealthPoints));
            OnPropertyChanged(nameof(Armor));
            OnPropertyChanged(nameof(Damage));
            OnPropertyChanged(nameof(EnnemyHealthPoints));
            OnPropertyChanged(nameof(EnnemyArmor));
            OnPropertyChanged(nameof(EnnemyDamage));
        }
    }
}
