using Joueur_Server.Models;
using Joueur_Server.Service;
using NUnit.Framework;

namespace Joueur_Server.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void Execute_Configuration_ConfigurationValide()
        {
            // Arrange
            int hp = 3;
            int armor = 4;
            int damage = 3;
            Robot bot = new Robot("bot");
            var gameService = new GameService();

            // Act
            bot.ConfigureRobot(hp, armor, damage);
            bool valid = gameService.DataValidation(bot);

            // Assert
            Assert.That(valid, Is.EqualTo(true));
        }

        [Test]
        public void Execute_Configuration_ConfigurationInvalide()
        {
            // Arrange
            int hp = 5;
            int armor = 9;
            int damage = 10;
            Robot bot = new Robot("bot");
            var gameService = new GameService();

            // Act
            bot.ConfigureRobot(hp, armor, damage);
            bool valid = gameService.DataValidation(bot);

            // Assert
            Assert.That(valid, Is.EqualTo(false));
        }

        [Test]
        public void Execute_Configuration_ConfigurationInvalideNegative()
        {
            // Arrange
            int hp = -2;
            int armor = 8;
            int damage = 4;
            Robot bot = new Robot("bot");
            var gameService = new GameService();

            // Act
            bot.ConfigureRobot(hp, armor, damage);
            bool valid = gameService.DataValidation(bot);

            // Assert
            Assert.That(valid, Is.EqualTo(false));
        }

        [Test]
        public void Execute_AttaquePuissante_AppliqueLesDegatsCorrects()
        {
            // Arrange
            var attacker = new Robot("Attacker") { Damage = 0, Energy = 50 };
            var target = new Robot("target") { Armor = 8, HealthPoints = 130 };
            var attack = new PowerfulAttack();

            // Act
            attack.Execute(attacker, target);

            // Assert
            Assert.That(target.HealthPoints, Is.EqualTo(38));
        }

        [Test]
        public void Execute_AttaquePuissante_ActionRefusee()
        {
            // Arrange
            var attacker = new Robot("Attacker") { Damage = 0, Energy = 0 };
            var target = new Robot("target") { Armor = 8, HealthPoints = 130 };
            var attack = new PowerfulAttack();

            // Act
            attack.Execute(attacker, target);

            // Assert
            Assert.That(target.HealthPoints, Is.EqualTo(130));
        }

        [Test]
        public void Execute_Defense_ReduitAttaque()
        {
            // Arrange
            var attacker = new Robot("Attacker") { Damage = 10 };
            var target = new Robot("target") { Armor = 0, HealthPoints = 10 };
            var attack = new PowerfulAttack();
            var defense = new Defense();

            // Act
            defense.Execute(target, attacker);
            attack.Execute(attacker, target);

            // Assert
            Assert.That(target.HealthPoints, Is.EqualTo(5));
        }

        [Test]
        public void Execute_Recharge_EnergyAugmentee()
        {
            // Arrange
            var attacker = new Robot("Attacker") { };
            var target = new Robot("target") { };
            var recharge = new Recharge();

            // Act
            recharge.Execute(attacker, target);

            // Assert
            Assert.That(attacker.Energy, Is.EqualTo(5));
        }

        [Test]
        public void Execute_Json_SerialisationFonctionne()
        {
            // Arrange
            var baseData = new Data() { Message = "test 1 test 2 brbrbrbrbr" };
            var server = new Server();

            // Act
            var serialisedData = baseData.ToJson();
            var extractedData = Data.FromJson(serialisedData);


            // Assert
            Assert.That(baseData.Message, Is.EqualTo(extractedData.Message));
        }

        [Test]
        public void Execute_AttaqueSimple_AppliqueLesDegatsCorrects()
        {
            // Arrange
            var attacker = new Robot("Attacker") { Damage = 16 };
            var target = new Robot("target") { Armor = 8, HealthPoints = 16 };
            var attack = new Attack();

            // Act
            attack.Execute(attacker, target);

            // Assert
            Assert.That(target.HealthPoints, Is.EqualTo(8));
        }
    }
}
