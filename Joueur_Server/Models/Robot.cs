using Joueur_Server.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class Robot
    {
		private string name;
        private int healthPoints;
        private int armor;
        private int damage;
        private int energy;
        private int defenseBonus;

        public string Name
		{
			get => name; 
			set 
			{
				ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
				name = value; 
			}
		}

		public int HealthPoints
		{
			get => healthPoints; 
			set 
			{
				if(value < 0) value = 0;
				healthPoints = value; 
			}
		}

		public int Armor
		{
			get => armor; 
			set => armor = value; 
		}

		public int Damage
		{
			get => damage; 
			set => damage = value; 
		}

		public int Energy
		{
			get => energy; 
			set => energy = value; 
		}

		public int DefenseBonus
		{
			get => defenseBonus; 
			set => defenseBonus = value; 
		}

        public Robot(string name)
        {
			this.name = name;
			HealthPoints = GameConstants.BASE_HEALTH_POINTS;
			Armor = GameConstants.BASE_ARMOR;
			Damage = GameConstants.BASE_DAMAGE;
			Energy = GameConstants.BASE_ENERGY;
			DefenseBonus = GameConstants.BASE_DEFENSE_BONUS;
        }

        public void ConfigureRobot(int hp, int armor, int damage)
		{
			if (hp + armor + damage != GameConstants.HABILITY_POINTS)
				throw new ArgumentException("Robot Must have spend the exact amount of hability points modification points");
			HealthPoints += hp * GameConstants.HEALTH_MULTIPLIER;
			Armor += armor * GameConstants.DEFENSE_MULTIPLIER;
			Damage += damage * GameConstants.DAMAGE_MULTIPLIER;
		}

		public void TakeDamage(int damage)
		{
			HealthPoints -= damage;
		}

		public bool IsAlive()
		{
			return HealthPoints <= 0;
		}
	}
}
