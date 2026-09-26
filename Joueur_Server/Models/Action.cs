using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
	public abstract class Action
	{
		private string _name;
        private int _energyCost;
        private int _bonus;

        public string Name
		{
			get => _name; 
			set => _name = value; 
		}

		public int EnergyCost
		{
			get => _energyCost; 
			set => _energyCost = value; 
		}

		public int Bonus
		{
			get => _bonus; 
			set => _bonus = value; 
		}

        protected Action(string name, int energyCost, int bonus)
        {
            _name = name;
			_energyCost = energyCost;
			_bonus = bonus;
        }

        public abstract void Execute(Robot player, Robot opponent);
	}
}
