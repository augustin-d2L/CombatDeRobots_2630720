using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class Defense : Action
    {
        public Defense() :base("Defense", 0, 0) { }

        public override void Execute(Robot player, Robot opponent)
        {
            player.DefenseBonus += 1;
        }
    }
}
