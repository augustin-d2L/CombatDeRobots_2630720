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
            int damage = (player.Damage - (opponent.Armor + opponent.DefenseBonus)) > 0 
                ? (player.Damage - (opponent.Armor + opponent.DefenseBonus)) : 0;
            opponent.HealthPoints -= damage;
            opponent.DefenseBonus = 0;
        }
    }
}
