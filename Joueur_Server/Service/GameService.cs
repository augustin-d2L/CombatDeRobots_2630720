using Joueur_Server.Common;
using Joueur_Server.Common.Enums;
using Joueur_Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Ribbon;

namespace Joueur_Server.Service
{
    public class GameService : IServiceGame
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
            player.Energy += GameConstants.PASSIVE_ENERGY_GAIN;
            return true;
        }

        public bool DataValidation(Robot robot)
        {
            bool noNegative = true;
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
            if (health < 0 || armor < 0 || damage < 0) noNegative = false;

            return ((health + armor + damage) == GameConstants.HABILITY_POINTS) && noNegative;
        }

        public string OperateAction(ActionCombat? action, Robot player, Robot opponent)
        {
            string message = "";
            switch (action)
            {
                case ActionCombat.ATTACK:
                    Random random = new Random();
                    if(random.Next(0, 100) >= GameConstants.ATTACK_CRITICAL_CHANCE)
                    {
                        Attack.Execute(player, opponent);
                        message = $"{player.Name} a fait ATTAQUE sur {opponent.Name}";
                    }
                    else
                    {
                        player.Damage *= 2;
                        Attack.Execute(player, opponent);
                        player.Damage /= 2;
                        message = $"{player.Name} a fait ATTAQUE CRITIQUE sur {opponent.Name}";
                    }
                    break;
                case ActionCombat.POWERFUL_ATTACK://TODO : ajouter la logique random d'echec
                    PowerfulAttack.Execute(player, opponent);
                    message = $"{player.Name} a fait ATTAQUE PUISSANTE sur {opponent.Name}";
                    break;
                case ActionCombat.DEFENSE://TODO : ajouter la logique random d'echec
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

        public (string equation, int answer) CreateEquation()
        {
            int val1 = new Random().Next(GameConstants.MINIMUM_RANGE_EQUATION, GameConstants.MAXIMUM_RANGE_EQUATION);
            int val2 = new Random().Next(GameConstants.MINIMUM_RANGE_EQUATION, GameConstants.MAXIMUM_RANGE_EQUATION);
            int op = new Random().Next(3);

            string equation = "";
            int answer = 0;
            switch (op)
            {
                case 0:
                    equation = $"{val1} + {val2} = ";
                    answer = val1 + val2;
                    break;
                case 1:
                    if (val1 >= val2)
                    {
                        equation = $"{val1} - {val2} = ";
                        answer = val1 - val2;
                    }
                    else
                    {
                        equation = $"{val2} - {val1} = ";
                        answer = val2 - val1;
                    }
                    break;
                case 2:
                    equation = $"{val1} * {val2} = ";
                    answer = val1 * val2;
                    break;
                default:
                    break;
            }

            return (equation, answer);
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
