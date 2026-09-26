using Joueur_Server.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    public class Defense : Action
    {
        public Defense() :base("Defense", 0, 0) { }

        public override void Execute(Robot player, Robot opponent)
        {
            player.DefenseBonus += GameConstants.ADD_BONUS;
        }
    }
}
