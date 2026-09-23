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

        public string OperateAction(ActionCombat action)
        {
            return null;
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
