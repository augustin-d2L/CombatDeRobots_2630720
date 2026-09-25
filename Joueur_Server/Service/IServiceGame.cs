using Joueur_Server.Common.Enums;
using Joueur_Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Service
{
    internal interface IServiceGame
    {
        //bool DataValidation(Robot sanitisation); //validation du robot
        string OperateAction(ActionCombat? action, Robot player, Robot opponent);

        public Models.Action Attack { get; set; }
        public Models.Action PowerfulAttack { get; set; }
        public Models.Action Defense { get; set; }
        public Models.Action Recharge { get; set; }
    }
}
