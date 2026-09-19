using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Service
{
    internal class IServiceGame
    {
        //bool DataValidation(Null, char sanitisation)
        //string OperateAction(ActionCombat action

        private Models.Action attack;
        private Models.Action powerfulAttack;
        private Models.Action defense;
        private Models.Action recharge;

        public Models.Action Attack
        {
            get { return attack; }
            init { attack = value; }
        }

        public Models.Action PowerfulAttack
        {
            get { return powerfulAttack; }
            init { powerfulAttack = value; }
        }

        public Models.Action Defense
        {
            get { return defense; }
            init { defense = value; }
        }

        public Models.Action Recharge
        {
            get { return recharge; }
            init { recharge = value; }
        }

        public IServiceGame()
        {
            Attack = new Models.Attack();
            PowerfulAttack = new Models.PowerfulAttack();
            Defense = new Models.Defense();
            Recharge = new Models.Recharge();
        }
    }
}
