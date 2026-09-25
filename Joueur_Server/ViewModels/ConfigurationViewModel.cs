using Joueur_Server.Common;
using Joueur_Server.Helpers;
using Joueur_Server.Models;
using Joueur_Server.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Joueur_Server.ViewModels
{
    internal class ConfigurationViewModel : BaseViewModel
    {
        private readonly Server _server;
        private readonly Action<Robot, Robot> _onBothReady;

        private string _robotName = "";
        public string RobotName
        {
            get => _robotName;
            set
            {
                if (SetProperty(ref _robotName, value))
                {
                    (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _isReady;
        public bool IsReady
        {
            get => _isReady;
            set
            {
                if (SetProperty(ref _isReady, value))
                {
                    (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private int _hpPoints;
        public int HpPoints { get => _hpPoints; private set => SetProperty(ref _hpPoints, value); }

        private int _armorPoints;
        public int ArmorPoints { get => _armorPoints; private set => SetProperty(ref _armorPoints, value); }

        private int _damagePoints;
        public int DamagePoints { get => _damagePoints; private set => SetProperty(ref _damagePoints, value); }

        private Robot? _currentRobot;
        public Robot? CurrentRobot { get => _currentRobot; private set => SetProperty(ref _currentRobot, value); }

        public int RemainingPoints => GameConstants.HABILITY_POINTS - (HpPoints + ArmorPoints + DamagePoints);
        public int PreviewHealthPoints => GameConstants.BASE_HEALTH_POINTS + HpPoints * GameConstants.HEALTH_MULTIPLIER;
        public int PreviewArmor => GameConstants.BASE_ARMOR + ArmorPoints * GameConstants.DEFENSE_MULTIPLIER;
        public int PreviewDamage => GameConstants.BASE_DAMAGE + DamagePoints * GameConstants.DAMAGE_MULTIPLIER;

        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand ConfirmCommand { get; }

        public ConfigurationViewModel(Server server, Action<Robot, Robot> onBothReady)
        {
            _server = server;
            _onBothReady = onBothReady;

            IncrementCommand = new RelayCommand(Increment, CanIncrement);
            DecrementCommand = new RelayCommand(Decrement, CanDecrement);
            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
        }

        private void Increment(object? statName)
        {
            switch (statName as string)
            {
                case "Hp": HpPoints++; break;
                case "Armor": ArmorPoints++; break;
                case "Damage": DamagePoints++; break;
            }
            NotifierChangementAllocation();
        }

        private void Decrement(object? statName)
        {
            switch (statName as string)
            {
                case "Hp": if (HpPoints > 0) HpPoints--; break;
                case "Armor": if (ArmorPoints > 0) ArmorPoints--; break;
                case "Damage": if (DamagePoints > 0) DamagePoints--; break;
            }
            NotifierChangementAllocation();
        }

        private bool CanIncrement(object? _) => RemainingPoints > 0 && !IsReady;
        private bool CanDecrement(object? statName) => !IsReady && (statName as string) switch
        {
            "Hp" => HpPoints > 0,
            "Armor" => ArmorPoints > 0,
            "Damage" => DamagePoints > 0,
            _ => false
        };

        private bool CanConfirm(object? _) => !string.IsNullOrWhiteSpace(RobotName) && RemainingPoints == 0 && !IsReady;
        private async void Confirm(object? _)
        {
            IsReady = true;
            var robot = new Robot(RobotName);
            robot.ConfigureRobot(HpPoints, ArmorPoints, DamagePoints);
            CurrentRobot = robot;
            //robot est pret donc envoie de DATA vide avec ServerIsReady = True
            //1 sendData ServerIsReady = True;
            await _server.SendData(new Data { ServerIsReady = true });

            //2 receuiveData RobotClient
            Data clientData = await _server.ReceiveData();
            Robot robotClient = clientData.RobotClient;

            //3 on verifie les config si y'a un problème on avise
            bool isValid = _server.Service.DataValidation(robotClient);
            /* verifier config robot dans gameService */

            await _server.SendData(new Data
            {
                RobotClient = robotClient,
                RobotServer = robot,
                PlayerIsValid = isValid,
            });

            //4 naviguer à la page de combat
            if (isValid)
            {
                _onBothReady(robot, robotClient);
            }
            else
            {
                throw new ArgumentException("Impossible de démarrer la partie.");//a changer
            }
        }

        private void NotifierChangementAllocation()
        {
            OnPropertyChanged(nameof(RemainingPoints));
            OnPropertyChanged(nameof(PreviewHealthPoints));
            OnPropertyChanged(nameof(PreviewArmor));
            OnPropertyChanged(nameof(PreviewDamage));
            (IncrementCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DecrementCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
