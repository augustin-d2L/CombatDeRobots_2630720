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
			if (hp + armor + damage != 10)
				throw new ArgumentException("Robot Must have 10 modification points");
			HealthPoints += hp * 10;//10 could be in constants as HP_MULITPLIER
			Armor += armor * 2;
			Damage += damage * 2;
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
