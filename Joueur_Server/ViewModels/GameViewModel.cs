using Joueur_Server.Common.Enums;
using Joueur_Server.Helpers;
using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Joueur_Server.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        private readonly Server _server;
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

        // Robot du server
        public string RobotName => _robotServer.Name;
        public int HealthPoints => _robotServer.HealthPoints;
        public int Armor => _robotServer.Armor;
        public int Damage => _robotServer.Damage;
        public int Energy => _robotServer.Energy;
        public int DefenseBonus => _robotServer.DefenseBonus;

        // Robot du client
        public string EnnemyRobotName => _robotClient.Name;
        public int EnnemyHealthPoints => _robotClient.HealthPoints;
        public int EnnemyArmor => _robotClient.Armor;
        public int EnnemyDamage => _robotClient.Damage;
        public int EnnemyEnergy => _robotClient.Energy;
        public int EnnemyDefenseBonus => _robotClient.DefenseBonus;

        public ICommand PlayTurnCommand { get; }
        public ICommand SelectActionCommand { get; }

        public System.Action<Data> NavigateToEndGame { get; }

        public GameViewModel(Server server, Robot robotServer, Robot robotClient, System.Action<Data> navigateToEndGame)
        {
            _server = server;
            _robotServer = robotServer;
            _robotClient = robotClient;
            NavigateToEndGame = navigateToEndGame;

            IsTurnToPlay = false;

            SelectActionCommand = new RelayCommand(SelectAction);
            PlayTurnCommand = new RelayCommand(PlayTurn, CanPlayTurn);

            _ = ListenToOpponent();
        }

        private bool CanPlayTurn(object? _) => IsTurnToPlay && SelectedAction != null && !ChoseAction;
        private async void PlayTurn(object? _)
        {
            ActionCombat action;
            Enum.TryParse<ActionCombat>(SelectedAction, out action);

            //traitement de l'action
            LastVersionData.Action = action;

            bool gameInProgress = _server.Service.PerformTurn(LastVersionData, IsTurnToPlay);

            //envois de l'action faite au client
            await _server.SendData(LastVersionData);

            //mise a jour display et retour en attente
            _robotClient = LastVersionData.RobotClient;
            _robotServer = LastVersionData.RobotServer;
            IsTurnToPlay = false;
            UpdateDisplay();

            if (gameInProgress) _ = ListenToOpponent();
            else NavigateToEndGame(LastVersionData);
        }

        private async Task ListenToOpponent()
        {
            //attendre l'action
            LastVersionData = await _server.ReceiveData();

            //traitement de l'action
            bool gameInProgress = _server.Service.PerformTurn(LastVersionData, IsTurnToPlay);

            //retour avec l'action faite
            await _server.SendData(LastVersionData);

            //update display
            _robotClient = LastVersionData.RobotClient;
            _robotServer = LastVersionData.RobotServer;
            IsTurnToPlay = true;
            UpdateDisplay();
            if (!gameInProgress) NavigateToEndGame(LastVersionData);
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
        }
    }
}
