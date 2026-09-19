using Joueur_Server.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joueur_Server.Models
{
    internal class Data
    {
        Robot RobotServer { get; set; }

        Robot RobotClient { get; set; }

        ActionCombat? Action { get; set; }

        string GameState { get; set; }

        bool PlayAgain { get; set; }

        public Robot Winner { get; set; }

        public string Message { get; set; }

        public bool ServerIsReady { get; set; }

        public bool PlayerIsValid { get; set; }
    }
}
