using Joueur_Client.Common.Enums;
using Joueur_Client.Helpers;
using Joueur_Client.Models;
using Joueur_Client.Service;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
                    (PlayTurnCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
        public string Message => LastVersionData?.Message ?? "";

        public ICommand PlayTurnCommand { get; }
        public ICommand SelectActionCommand { get; }

        public System.Action<Data> NavigateToEndGame { get; }
        private readonly System.Action<string?> _navigateToMainMenu;


        public GameViewModel(Client client, Robot robotServer, Robot robotClient, System.Action<Data> navigateToEndGame, System.Action<string?> navigateToMainMenu)
        {
            _client = client;
            _robotServer = robotServer;
            _robotClient = robotClient;
            NavigateToEndGame = navigateToEndGame;
            _navigateToMainMenu = navigateToMainMenu;

            IsTurnToPlay = false;
            LastVersionData = new Data { RobotClient = _robotClient, RobotServer = _robotServer };

            SelectActionCommand = new RelayCommand(SelectAction);
            PlayTurnCommand = new RelayCommand(PlayTurn, CanPlayTurn);

            _ = ListenToOpponent();
        }

        private bool CanPlayTurn(object? _) => IsTurnToPlay && SelectedAction != null;
        private async void PlayTurn(object? _)
        {
            try
            {
                IsTurnToPlay = false;

                ActionCombat action;
                Enum.TryParse<ActionCombat>(SelectedAction, out action);

                //Envoyer à l'autre l'action
                LastVersionData.Action = action;
                await _client.SendData(LastVersionData);

                //Recevoir l'action 
                LastVersionData = await _client.ReceiveData();

                if (LastVersionData.GameState == GameState.done)
                {
                    //fim de partie
                    NavigateToEndGame(LastVersionData);
                }
                else
                {
                    _robotClient = LastVersionData.RobotClient;
                    _robotServer = LastVersionData.RobotServer;

                    UpdateDisplay();

                    _ = ListenToOpponent();
                }
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le serveur");
            }
        }

        private async Task ListenToOpponent()
        {
            try
            {
                LastVersionData = await _client.ReceiveData();

                if (LastVersionData.GameState == GameState.done)
                {
                    //fim de partie
                    NavigateToEndGame(LastVersionData);
                }

                _robotClient = LastVersionData.RobotClient;
                _robotServer = LastVersionData.RobotServer;
                IsTurnToPlay = true;
                UpdateDisplay();
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le serveur");
            }
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
