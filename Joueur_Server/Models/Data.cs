using Joueur_Server.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Joueur_Server.Models
{
    public class Data
    {
        public Robot RobotServer { get; set; }

        public Robot RobotClient { get; set; }

        public ActionCombat? Action { get; set; }

        public GameState GameState { get; set; }

        public bool PlayAgain { get; set; }

        public string Winner { get; set; }

        public string Message { get; set; }

        public bool ServerIsReady { get; set; }

        public bool PlayerIsValid { get; set; }

        public string MathProblem { get; set; }

        public int ClientCalculAnswer { get; set; }

        public Data()
        {
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Data FromJson(string json)
        {
            return JsonSerializer.Deserialize<Data>(json) ?? throw new InvalidOperationException("Désérialisation impossible : JSON invalide.");
        }
    }
}
