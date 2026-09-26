# CombatDeRobots_2630720

Application WPF pour maîtriser les sockets synchrones et la sérialisation/désérialisation de données dans le cadre du cours *Développement d'application Expert (420-E80-CH)*. L'application permet de se connecter à un autre joueur, de configurer un robot et de se battre en tour par tour.

## Description

L'application fonctionne autour des sockets synchrones. Le serveur est celui qui héberge et détient la vérité du jeu. Le client peut se connecter au serveur grâce à son IP et son port. Ensuite, chacun devra configurer son robot, et une fois la configuration validée de chaque côté, il y aura une étape de combat en tour par tour. La partie se termine quand la vie du robot d'un des deux joueurs atteint zéro, et finalement le client décidera si les deux rejouent la partie ou s'ils quittent.

## Fonctionnalités

- Le serveur peut se mettre en attente et attendre une connexion de la part d'un client
- Le client peut se connecter au serveur grâce à son IP et son port
- Les deux pourront faire leur configuration
- Ils se battront au tour par tour jusqu'à ce que l'un d'eux meure
- Un écran de fin sera affiché et le client pourra choisir de recommencer la partie ou de quitter le jeu

## Modèles de données

Les schémas ci-dessous représentent les interactions entre les classes, le déroulement de l'application et les cas d'utilisation présents dans l'application.

### Diagrammes de Classes

```mermaid
classDiagram
class client {
-socket
+sendData ()
+receiveData ()
+CloseConnection()
}

class server {
-listener
+sendData()
+receiveData()
+CloseConnection()
}

 class Data {
        -Robot robotServer
        -Robot robotClient
        -ActionCombat action NULLABLE
        -string GameState
        -bool PlayAgain
        -string Winner
        -string Message
        -bool ServerIsReady
        -bool PlayerIsValid
        
    }
    
Data <-- client
Data <-- server


    class IServiceGame {
        -bool PerformTurn(Data data)
        -bool DataValidation (Robot robot)
        -string OperateAction (ActionCombat action)
    }

    class GameService {
    }

     GameService <|.. IServiceGame

     GameService <-- server

class GameConstants {
        -const int HABILITY_POINTS = 10
        -const int BASE_HEALTH_POINTS = 100
        -const int BASE_ARMOR = 0
        -const int BASE_DAMAGE = 10
        -const int BASE_ENERGY = 0
        -const int BASE_DEFENSE_BONUS = 0
        -const int DEFENSE_MULTIPLIER = 2
        -const int ENERGY_MULTIPLIER = 2
        -const int DAMAGE_MULTIPLIER = 2
        -const int HEALTH_MULTIPLIER = 10
        -const int ADD_BONUS = 5
        -const int ADD_ENERGY = 5
        -const string EOM_DELIMITER = "<|EOM|>"
    }
       

    class Robot {
        -String name
        -int healthPoints
        -int armor
        -int damage
        -int energy
        -int defenseBonus
        +configureRobot(hp, armor, damage)
        +takeDamage(damage)
        +isAlive() bool
    }

class ActionCombat {
        <<enumeration>>
        ATTACK
        POWERFUL_ATTACK
        DEFENSE
        RECHARGE
    }

    class Action {
        <<abstract>>
        #String name
        #int energyCost
        #int bonus
        +execute(Robot player, Robot opponent)*
    }

    class Attack {
        +execute(player, opponent)
    }

    class PowerfulAttack {
        -int energyCost
        -int damageMultiplier
        +execute(player, opponent)
    }

    class Defense {
        -int defenseBonus
        +execute(player)
    }

    class Recharge {
        -int energyRecovered
        +execute(player)
    }

    

    Action <|-- Attack
    Action <|-- PowerfulAttack
    Action <|-- Defense
    Action <|-- Recharge

    IServiceGame "1" --> "4" Action : chooses


```

### Diagrammes de séquence

```mermaid
sequenceDiagram
    participant Client as client
    participant Server as server

    Note over Server: server écoute (listener) en continu

    Client->>Server: Connexion socket
    Server-->>Client: Connexion établie

    Client->>Client: Configure Robot (name, healthPoints, armor, damage)
    Server->>Server: Configure Robot (name, healthPoints, armor, damage)

    Client->>Server: Data { robot client, player_is_valid }
    Server->>Server: IService.DataValidation() (total points, null, sanitisation)

    alt Configuration valide
        Server->>Server: Robot serveur configuré, player_is_ready = true
    else Configuration invalide
        Server->>Client: Data { message: erreur configuration }
        Client->>Server: Data { robot client corrigé, player_is_valid }
    end

    Note over Client,Server: Les deux configurations sont secrètes l'une envers l'autre

    Server->>Server: Game.GameState = "En cours"
    Server->>Client: Data { état de la partie: En cours }

    loop Combat tour par tour, jusqu'à GameState = "Terminé"
        Client->>Server: Data { action: Attack / PowerfulAttack / Defense / Recharge }
        Server->>Server: IService.ActionOperation() -> Action.execute(robot client, robot serveur)
        Server->>Server: Met à jour healthPoints, energy, defenseBonus des deux robots
        Server->>Client: Data { robot client, robot serveur, message }
        Client->>Client: Met à jour son affichage
    end

    Server->>Server: Game détecte fin (healthPoints <= 0), fixe gagnant
    Server->>Client: Data { état de la partie: Terminé, gagnant }

    Client->>Server: Data { play_again: true/false }
    alt play_again = true
        Server->>Server: Réinitialise healthPoints/energy/defenseBonus des 2 Robot (même name, même répartition de points)
        Server->>Server: Game.GameState = "En cours"
        Server->>Client: Data { robot client, robot serveur, état de la partie: En cours }
        Note over Client,Server: Reconfiguration : on retourne à la configuration des robots
    else play_again = false
        Server->>Server: retourne en attente (listener)
    end
```

### Diagrammes de cas d'utilisation

```mermaid
flowchart LR
    subgraph SYS_SERVER["System: Server Application"]
        direction TB
        S1(["Host a game"])
        S2(["Configure their robot"])
        S3(["Confirm their configuration"])
        S3v(["Validate the received configuration"])
        S5(["Play a combat turn"])
        S5v(["Validate the received action"])
        S7(["Detect end of game"])
        S9(["Handle a client disconnection"])
    end

    subgraph SYS_CLIENT["System: Client Application"]
        direction TB
        C1(["Connect to the server"])
        C2(["Configure their robot"])
        C3(["Confirm their configuration"])
        C5(["Decide to replay or quit"])
        C6(["Play a combat turn"])
        C7(["Handle a lost connection"])
        C8(["validate attack client side"])
    end

    HostPlayer(("Player (host)"))
    ClientPlayer(("Player (client)"))

    HostPlayer --- S1
    HostPlayer --- S2
    HostPlayer --- S3
    HostPlayer --- S5

    ClientPlayer --- C1
    ClientPlayer --- C2
    ClientPlayer --- C3
    ClientPlayer --- C5
    ClientPlayer --- C6

    S3 -. include .-> S3v
    S5 -. include .-> S5v
    S1 -. extend .-> S9
    C1 -. extend .-> C7
    C6 -. include .-> C8
```

## Structure du projet

```
CombatDeRobots_2630720/
├── Joueur_Client/
│   ├── Common/       # enumérable et constante
│   ├── Helpers/      # RelayCommand.cs
│   ├── Models/       
│   ├── Service/      # classes de services
│   ├── ViewModels/   
|   └── Views/        
├── Joueur_Server/
│   ├── Common/       # enumérable et constante
│   ├── Helpers/      # RelayCommand.cs
│   ├── Models/       
│   ├── Service/      # classes de services
│   ├── ViewModels/   
|   └── Views/ 
└── Joueur_Server.Test/
```

## Technologies

- C# / WPF
- XAML pour l'affichage
- Sockets synchrones

## Auteur

Projet réalisé en deux parties :
- Première partie : analyse réalisée en groupe
- Seconde partie : développement réalisé seul

Le tout dans le cadre du cours 420-E80-CH.
