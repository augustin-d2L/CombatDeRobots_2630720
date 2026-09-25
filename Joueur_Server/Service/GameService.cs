using Joueur_Server.Common;
using Joueur_Server.Common.Enums;
using Joueur_Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Ribbon;

namespace Joueur_Server.Service
{
    internal class GameService : IServiceGame
    {
        public bool PerformTurn(Data data, bool serverTurn)
        {
            ActionCombat? action = data.Action;
            Robot player;
            Robot opponent;

            if (serverTurn)
            {
                player = data.RobotServer;
                opponent = data.RobotClient;
            }
            else
            {
                player = data.RobotClient;
                opponent = data.RobotServer;
            }
            data.Message = OperateAction(action, player, opponent);
            if (opponent.HealthPoints <= 0)
            {
                data.Winner = player.Name;
                data.Message = $"{data.Winner} est le gagnant, il a battu {opponent.Name}";
                return false;
            }
            return true;
        }

        public bool DataValidation(Robot robot)
        {
            //decortiquer le robot pour voir s'il est valide
            int health = robot.HealthPoints;
            int armor = robot.Armor;
            int damage = robot.Damage;

            health = health - GameConstants.BASE_HEALTH_POINTS;
            armor = armor - GameConstants.BASE_ARMOR;
            damage = damage - GameConstants.BASE_DAMAGE;

            health = health / GameConstants.HEALTH_MULTIPLIER;
            armor = armor / GameConstants.DEFENSE_MULTIPLIER;
            damage = damage / GameConstants.DAMAGE_MULTIPLIER;
            //enlever la valeur par defaut et ensuite diviser pour additionner et ça doit être egal à la valeur de la constante

            return (health + armor + damage) == GameConstants.HABILITY_POINTS;
        }

        public string OperateAction(ActionCombat? action, Robot player, Robot opponent)
        {
            string message = "";
            switch (action)
            {
                case ActionCombat.ATTACK:
                    Attack.Execute(player, opponent);
                    message = $"{player.Name} a fait ATTAQUE sur {opponent.Name}";
                    break;
                case ActionCombat.POWERFUL_ATTACK:
                    PowerfulAttack.Execute(player, opponent);
                    message = $"{player.Name} a fait ATTAQUE PUISSANTE sur {opponent.Name}";
                    break;
                case ActionCombat.DEFENSE:
                    Defense.Execute(player, opponent);
                    message = $"{player.Name} a fait DEFENSE et a gagné de la défense bonus";
                    break;
                case ActionCombat.RECHARGE:
                    Recharge.Execute(player, opponent);
                    message = $"{player.Name} a fait RECHARGE et a gagné de l'energie";
                    break;
                default:
                    message = "Action inconnue";
                    break;
            }
            return message;
        }

        public Models.Action Attack { get; set; }
        public Models.Action PowerfulAttack { get; set; }
        public Models.Action Defense { get; set; }
        public Models.Action Recharge { get; set; }

        public GameService()
        {
            Attack = new Models.Attack();
            PowerfulAttack = new Models.PowerfulAttack();
            Defense = new Models.Defense();
            Recharge = new Models.Recharge();
        }
    }
}
