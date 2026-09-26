using Joueur_Server.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    public class PowerfulAttack : Action
    {
        public PowerfulAttack() :base("Powerful Attack", 0, 0) { }

        private readonly int damageMultiplier = GameConstants.ENERGY_MULTIPLIER;

        public override void Execute(Robot player, Robot opponent)
        {
            int damage = (player.Damage + (player.Energy * damageMultiplier) - (opponent.Armor + opponent.DefenseBonus)) > 0 
                ? (player.Damage + (player.Energy * damageMultiplier) - (opponent.Armor + opponent.DefenseBonus)) : 0;
            opponent.HealthPoints -= damage;
            opponent.DefenseBonus = 0;
            player.Energy = 0;
        }
    }
}
