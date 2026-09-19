using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class Attack : Action
    {
        public Attack() :base("Attack", 0, 0) { }

        public override void Execute(Robot player, Robot opponent)
        {
            opponent.HealthPoints -= (player.Damage - (opponent.Armor + opponent.DefenseBonus));
            opponent.DefenseBonus = 0;
        }
    }
}
