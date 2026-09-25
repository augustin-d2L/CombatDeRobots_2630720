using Joueur_Server.Common.Enums;
using Joueur_Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Service
{
    internal class GameService : IServiceGame
    {
        public bool PerformTurn(Data data)
        {
            return false;
        }

        public string OperateAction(ActionCombat? action, Robot player, Robot opponent)
        {
            string message = "";
            switch (action)
            {
                case ActionCombat.ATTACK:
                    Attack.Execute(player, opponent);
                    break;
                case ActionCombat.POWERFUL_ATTACK:
                    PowerfulAttack.Execute(player, opponent);
                    break;
                case ActionCombat.DEFENSE:
                    Defense.Execute(player, opponent);
                    break;
                case ActionCombat.RECHARGE:
                    Recharge.Execute(player, opponent);
                    break;
                default:
                    message = "Action is not known";
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
