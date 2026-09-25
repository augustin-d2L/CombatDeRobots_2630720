using Joueur_Client.Common.Enums;
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
        private Robot _robotServer;
        private Robot _robotClient;

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
                    (PlayTurnCommand as RelayCommand)?.RaiseCanExecuteChanged(); //TODO : j'essaie de dire que quand une action est choisie le bouton se grise
                }
            }
        }

        private string? _selectedAction;
        public string? SelectedAction// peut - être moyen de directement convertir ici
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

        public Data LastVersionData { get; set; }

        // Robot du client
        public string RobotName => _robotClient.Name;
        public int HealthPoints => _robotClient.HealthPoints;
        public int Armor => _robotClient.Armor;
        public int Damage => _robotClient.Damage;
        public int Energy => _robotClient.Energy;
        public int DefenseBonus => _robotClient.DefenseBonus;

        // Robot du server
        public string EnnemyRobotName => _robotServer.Name;
        public int EnnemyHealthPoints => _robotServer.HealthPoints;
        public int EnnemyArmor => _robotServer.Armor;
        public int EnnemyDamage => _robotServer.Damage;
        public int EnnemyEnergy => _robotServer.Energy;
        public int EnnemyDefenseBonus => _robotServer.DefenseBonus;

        // Message
        public string Message => LastVersionData.Message;

        public ICommand PlayTurnCommand { get; }
        public ICommand SelectActionCommand { get; }

        public System.Action<Data> NavigateToEndGame { get; }

        public GameViewModel(Client client, Robot robotServer, Robot robotClient, System.Action<Data> navigateToEndGame)
        {
            _client = client;
            _robotServer = robotServer;
            _robotClient = robotClient;
            NavigateToEndGame = navigateToEndGame;

            IsTurnToPlay = true;
            LastVersionData = new Data { RobotClient = _robotClient, RobotServer = _robotServer };

            SelectActionCommand = new RelayCommand(SelectAction);
            PlayTurnCommand = new RelayCommand(PlayTurn, CanPlayTurn);
        }

        private bool CanPlayTurn(object? _) => IsTurnToPlay && SelectedAction != null && !ChoseAction;
        private async void PlayTurn(object? _)
        {
            ActionCombat action;
            Enum.TryParse<ActionCombat>(SelectedAction, out action);

            //Envoyer à l'autre l'action
            LastVersionData.Action = action;
            await _client.SendData(LastVersionData);//TODO il faut mettre un message pour l,afficher au client et dire au server ce qui c'est passé

            //Recevoir l'action 
            LastVersionData = await _client.ReceiveData();

            if (LastVersionData.Winner != null)
            {
                //fim de partie
                NavigateToEndGame(LastVersionData);
            }

            _robotClient = LastVersionData.RobotClient;
            _robotServer = LastVersionData.RobotServer;

            IsTurnToPlay = false;
            UpdateDisplay();

            if (LastVersionData.Winner != null)
            {
                NavigateToEndGame(LastVersionData);
            }
            else
            {
                _ = ListenToOpponent();
            }
        }

        private async Task ListenToOpponent()
        {
            LastVersionData = await _client.ReceiveData();

            if(LastVersionData.Winner != null)
            {
                //fim de partie
                LastVersionData.RobotClient.Name = "FIN";
            }

            _robotClient = LastVersionData.RobotClient;
            _robotServer = LastVersionData.RobotServer;
            IsTurnToPlay = true;
            UpdateDisplay();
        }

        private void SelectAction(object? actionName)
        {
            SelectedAction = actionName as string;
        }

        private void UpdateDisplay()
        {
            OnPropertyChanged(nameof(RobotName));
            OnPropertyChanged(nameof(HealthPoints));
            OnPropertyChanged(nameof(Armor));
            OnPropertyChanged(nameof(Damage));
            OnPropertyChanged(nameof(Energy));
            OnPropertyChanged(nameof(DefenseBonus));
            OnPropertyChanged(nameof(EnnemyRobotName));
            OnPropertyChanged(nameof(EnnemyHealthPoints));
            OnPropertyChanged(nameof(EnnemyArmor));
            OnPropertyChanged(nameof(EnnemyDamage));
            OnPropertyChanged(nameof(EnnemyEnergy));
            OnPropertyChanged(nameof(EnnemyDefenseBonus));

            OnPropertyChanged(nameof(Message));
        }
    }
}
