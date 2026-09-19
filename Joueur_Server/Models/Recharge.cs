using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class Recharge : Action
    {
        public Recharge() :base("Recharge", 0, 0) { }

        public override void Execute(Robot player, Robot opponent)
        {
            player.DefenseBonus += 1;
        }
    }
}
