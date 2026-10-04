using Joueur_Client.Common;
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
    internal class ConfigurationViewModel : BaseViewModel
    {
        private readonly Client _client;
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
        public int HpBonus => HpPoints * GameConstants.HEALTH_MULTIPLIER;
        public int ArmorBonus => ArmorPoints * GameConstants.DEFENSE_MULTIPLIER;
        public int DamageBonus => DamagePoints * GameConstants.DAMAGE_MULTIPLIER;

        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand ConfirmCommand { get; }

        private readonly Action<string?> _navigateToMainMenu;

        public ConfigurationViewModel(Client client, Action<Robot, Robot> onBothReady, Action<string> navigateToMainMenu)
        {
            _client = client;

            _onBothReady = onBothReady;
            _navigateToMainMenu = navigateToMainMenu;

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
            try
            {
                IsReady = true;
                var robot = new Robot(RobotName);
                robot.ConfigureRobot(HpPoints, ArmorPoints, DamagePoints);
                CurrentRobot = robot;
                //robot est pret donc envoie de DATA vide avec ServerIsReady = True; TODO : mettre la condition pour attendre serverisready et pas n'importeQuoi
                //1 receiveData ServerIsReady;
                await _client.ReceiveData();

                //2 sendData RobotClient
                //TODO : ajouter la validation if server is ready
                await _client.SendData(new Data { RobotClient = robot });

                //3 attendre le retour de config et si y'a un problème on avise
                //4 naviguer à la page de combat si player is valid
                Data serverData = await _client.ReceiveData();

                if (serverData.PlayerIsValid)
                {
                    _onBothReady(serverData.RobotServer, robot);
                }
                else
                {
                    IsReady = false;
                }
            }
            catch (SocketException)
            {
                _navigateToMainMenu("Connexion perdue avec le serveur");

            }
        }

        private void NotifierChangementAllocation()
        {
            OnPropertyChanged(nameof(RemainingPoints));
            OnPropertyChanged(nameof(PreviewHealthPoints));
            OnPropertyChanged(nameof(PreviewArmor));
            OnPropertyChanged(nameof(PreviewDamage));
            OnPropertyChanged(nameof(HpBonus));
            OnPropertyChanged(nameof(ArmorBonus));
            OnPropertyChanged(nameof(DamageBonus));
            (IncrementCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DecrementCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
