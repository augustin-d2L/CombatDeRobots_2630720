using Joueur_Server.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class PowerfulAttack : Action
    {
        public PowerfulAttack() :base("Powerful Attack", 0, 0) { }

        private readonly int damageMultiplier = GameConstants.ENERGY_MULTIPLIER;

        public override void Execute(Robot player, Robot opponent)
        {
            opponent.HealthPoints -= (player.Damage + (player.Energy * damageMultiplier) - (opponent.Armor + opponent.DefenseBonus));
            opponent.DefenseBonus = 0;
            player.Energy = 0;
        }
    }
}
