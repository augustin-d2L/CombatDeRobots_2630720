using Joueur_Server.Helpers;
using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        private readonly Server _server;
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


        // Robot du server
        public string RobotName => _robotServer.Name;
        public int HealthPoints => _robotServer.HealthPoints;
        public int Armor => _robotServer.Armor;
        public int Damage => _robotServer.Damage;

        // Robot du client
        public string EnnemyRobotName => _robotClient.Name;
        public int EnnemyHealthPoints => _robotClient.HealthPoints;
        public int EnnemyArmor => _robotClient.Armor;
        public int EnnemyDamage => _robotClient.Damage;

        public GameViewModel(Server server, Robot robotServer, Robot robotClient)
        {
            _server = server;
            _robotServer = robotServer;
            _robotClient = robotClient;

            IsTurnToPlay = false;
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
