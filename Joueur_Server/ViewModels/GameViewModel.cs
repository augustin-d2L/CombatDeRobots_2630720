using Joueur_Server.Common.Enums;
using Joueur_Server.Helpers;
using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
                    (PlayTurnCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _messageToSend = "";
        public string MessageToSend
        {
            get => _messageToSend;
            set
            {
                if (SetProperty(ref _messageToSend, value))
                {
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

        // Message
        public string Message => LastVersionData?.Message ?? "";

        public ICommand PlayTurnCommand { get; }
        public ICommand SelectActionCommand { get; }

        private readonly System.Action<Data> _navigateToEndGame;
        private readonly System.Action<string?> _navigateToMainMenu;
        private readonly Func<string, Task<int>> _askQuestion;


        public GameViewModel(Server server, Robot robotServer, Robot robotClient, System.Action<Data> navigateToEndGame, System.Action<string?> navigateToMainMenu, Func<string, Task<int>> askQuestion)
        {
            _server = server;
            _robotServer = robotServer;
            _robotClient = robotClient;
            _navigateToEndGame = navigateToEndGame;
            _navigateToMainMenu = navigateToMainMenu;
            _askQuestion = askQuestion;

            IsTurnToPlay = true;
            LastVersionData = new Data { RobotClient = _robotClient, RobotServer = _robotServer };

            SelectActionCommand = new RelayCommand(SelectAction);
            PlayTurnCommand = new RelayCommand(PlayTurn, CanPlayTurn);
        }

        private bool CanPlayTurn(object? _) => IsTurnToPlay && SelectedAction != null;
        private async void PlayTurn(object? _)
        {
            try
            {
                IsTurnToPlay = false;

                ActionCombat action;
                Enum.TryParse<ActionCombat>(SelectedAction, out action);

                //traitement de l'action
                LastVersionData.Action = action;

                // if action = defense
                if(LastVersionData.Action == ActionCombat.DEFENSE)
                {
                    // repondre à un calcul
                    var arithmeticExpression = _server.Service.CreateArithmeticExpression();
                    string question = $"Résoudre le calcul suivant : {arithmeticExpression.equation}";
                    int answer = await _askQuestion(question);
                    bool success = answer == arithmeticExpression.answer;
                    if (success)
                    {
                        if (!_server.Service.PerformTurn(LastVersionData, true))
                            LastVersionData.GameState = GameState.done;
                    }
                    else
                        LastVersionData.Message = $"{LastVersionData.RobotServer.Name} a échoué la défense.";
                }
                if(LastVersionData.Action != ActionCombat.DEFENSE)
                {
                    if (!_server.Service.PerformTurn(LastVersionData, true))
                        LastVersionData.GameState = GameState.done;
                }
                

                if (!MessageToSend.IsWhiteSpace())
                    LastVersionData.Message = MessageToSend;

                //envois de l'action faite au client
                LastVersionData.ArithmeticExpression = null;
                await _server.SendData(LastVersionData);

                //mise a jour display et retour en attente
                _robotClient = LastVersionData.RobotClient;
                _robotServer = LastVersionData.RobotServer;
                MessageToSend = "";
                UpdateDisplay();

                if (LastVersionData.GameState != GameState.done) _ = ListenToOpponent();
                else
                {
                    _navigateToEndGame(LastVersionData);
                }
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le client");
            }
        }

        private async Task ListenToOpponent()
        {
            try
            {
                //attendre l'action
                LastVersionData = await _server.ReceiveData();
                string messageReceived = "";

                if (!LastVersionData.Message.IsWhiteSpace())
                {
                    messageReceived = LastVersionData.Message;
                }

                //ajoute des robots depuis la mémoire
                LastVersionData.RobotServer = _robotServer;
                LastVersionData.RobotClient = _robotClient;

                if(LastVersionData.Action == ActionCombat.DEFENSE)
                {
                    var arithmeticExpression = _server.Service.CreateArithmeticExpression();
                    LastVersionData.ArithmeticExpression = arithmeticExpression.equation;

                    await _server.SendData(LastVersionData);
                    LastVersionData = await _server.ReceiveData();

                    if(LastVersionData.ArithmeticExpressionAnswer == arithmeticExpression.answer)
                    {
                        if (!_server.Service.PerformTurn(LastVersionData, false))
                            LastVersionData.GameState = GameState.done;
                    }
                    else
                        LastVersionData.Message = $"{LastVersionData.RobotClient.Name} a échoué la défense.";
                }
                if(LastVersionData.Action != ActionCombat.DEFENSE)
                {
                    if (!_server.Service.PerformTurn(LastVersionData, false))
                        LastVersionData.GameState = GameState.done;
                }
                if (!messageReceived.IsWhiteSpace())
                {
                    LastVersionData.Message = messageReceived;
                }

                //retour avec l'action faite
                LastVersionData.ArithmeticExpression = null;
                await _server.SendData(LastVersionData);

                //update display
                _robotClient = LastVersionData.RobotClient;

                IsTurnToPlay = true;

                UpdateDisplay();
                if (LastVersionData.GameState == GameState.done) _navigateToEndGame(LastVersionData);
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le client");
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
