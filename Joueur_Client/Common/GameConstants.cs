using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Client.Common
{
    public static class GameConstants
    {
        public const int HABILITY_POINTS = 10;
        public const int BASE_HEALTH_POINTS = 100;
        public const int BASE_ARMOR = 0;
        public const int BASE_DAMAGE = 10;
        public const int BASE_ENERGY = 0;
        public const int BASE_DEFENSE_BONUS = 0;

        public const int DEFENSE_MULTIPLIER = 2;
        public const int ENERGY_MULTIPLIER = 2;
        public const int DAMAGE_MULTIPLIER = 2;
        public const int HEALTH_MULTIPLIER = 10;

        public const int ADD_BONUS = 5;
        public const int ADD_ENERGY = 5;

        public const string EOM_DELIMITER = "<|EOM|>";

        public const int ATTACK_CRITICAL_CHANCE = 20;
        public const int POWERFUL_ATTACK_FAIL_CHANCE = 25;
        public const int DEFENSE_FAIL_CHANCE = 10;

        public const int PASSIVE_ENERGY_GAIN = 1;
        public const int MINIMUM_RANGE_ARITHMETIC_EXPRESSION = 0;
        public const int MAXIMUM_RANGE_ARITHMETIC_EXPRESSION = 100;
    }
}
