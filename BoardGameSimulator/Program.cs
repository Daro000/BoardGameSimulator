// See https://aka.ms/new-console-template for more information

class Player
{
    public string Name {get; set;}
    public int Position {get; set;}
    public int Score {get; set;}

    public Player(string name, int position, int score)
    {
        Name = name;
        Position = position;
        Score = score;
    }

    public void RollDiceAndMove()
    {
        Random random = new Random();
        int steps = random.Next(1, 7);
        Position += steps;
        Console.WriteLine($"{Name} przesunął się o {steps} pola i jest teraz na pozycji {Position} ");
    }

    public void UpdateScore(int points)
    {
        Score += points;
        Console.WriteLine($"{Name} zdobył {points} punktów. Aktualny wynik: {Score} ");
    }
}

class Board
{
    public int Size { get; set; }
    public Dictionary<int, int> Rewards { get; set; }

    public Board(int size)
    {
        Size = size;
        Rewards = new Dictionary<int, int>();
    }

    public void GenerateRewards(int rewardCount)
    {
        Random random = new Random();
        
        
        for (int i = 0; i < rewardCount; i++)
        {
            int position = random.Next(1, Size + 1);
            int reward = random.Next(1, 101); 
            
            
            if (!Rewards.ContainsKey(position))
            {
                Rewards[position] = reward;
            }
        }
    }
    public bool CheckForReward(int position)
    {
        return Rewards.ContainsKey(position);  
    }

    
    public int GetReward(int position)
    {
        if (Rewards.ContainsKey(position))
        {
            return Rewards[position];  
        }
        return 0;  
    }
}

delegate void SpecialEvent(Player player);
class Game
{
    public event SpecialEvent OnSpecialTile;
    public List<Player> Players { get; set; }
    public Board Board { get; set; }
    public int CurrentTurn { get; set; }
    public int TotalTurns { get; set; }
    private Dictionary<Player, IPlayerType> PlayerTypes;

    public Game(List<Player>players, Board board, Dictionary<Player, IPlayerType> playerTypes)
    {
        Players = players;
        Board = board;
        PlayerTypes = playerTypes;
        CurrentTurn = 0;
        TotalTurns = 0;
    }

    public void StartGame()
    {
        Console.WriteLine("Gra rozpoczęta");
        while (!IsGameOver())
        {
            TakeTurn();
        }

        DisplayResults();
    }

    public void TakeTurn()
    {
        Player currentPlayer = Players[CurrentTurn];
        Console.WriteLine($"Tura gracza {currentPlayer.Name}");
        
        currentPlayer.RollDiceAndMove();

        if (Board.CheckForReward(currentPlayer.Position))
        {
            int reward = Board.GetReward(currentPlayer.Position);
            Console.WriteLine($"{currentPlayer.Name} trafił na pole {currentPlayer.Position} i zdobył nagrode: {reward} ");
            currentPlayer.UpdateScore(reward);
            
            OnSpecialTile?.Invoke(currentPlayer);
        }
        else
        {
            Console.WriteLine($"{currentPlayer.Name} nie trafił na pole z nagrodą");
        }
        
        if (PlayerTypes.ContainsKey(currentPlayer))
        {
            PlayerTypes[currentPlayer].SpecialMove(currentPlayer);
        }
        
        CurrentTurn = (CurrentTurn + 1) % Players.Count;
        
        TotalTurns++;
    }

    public bool IsGameOver()
    {
        return TotalTurns >= 10;
    }

    public void DisplayResults()
    {
        Console.WriteLine("Koniec gry!! Wyniki:");
        foreach (var player in Players)
        {
            Console.WriteLine($"{player.Name}: {player.Score} punktów");
        }
    }
}

interface IPlayerType
{
    void SpecialMove(Player player);
}

class Warrior : IPlayerType
{
    public void SpecialMove(Player player)
    {
        player.UpdateScore(50);
        Console.WriteLine($"{player.Name} Wojownik wykonuje specjalny cios i zyskuje 50 pkt");
    }
}

class Mage : IPlayerType
{
    public void SpecialMove(Player player)
    {
        Console.WriteLine($"{player.Name} rzuca zaklecie i przesuwa sie o 2 pola");
        player.Position += 2;
    }
}

class Healer : IPlayerType
{
    public void SpecialMove(Player player)
    {
        Console.WriteLine($"{player.Name} leczy sie i dodaje sobie 20 pkt");
        player.UpdateScore(20);
    }
    
}


class Program
{
    static void Main(string[] args)
    {
        var player1 = new Player("boro", 0, 0);
        var player2 = new Player("jolo", 0, 0);
        var player3 = new Player("daro", 0, 0);

        Dictionary<Player, IPlayerType> playerTypes = new Dictionary<Player, IPlayerType>
        {
            { player1, new Warrior() },
            { player2, new Mage() },
            { player3, new Healer() }
        };
        
        Board board = new Board(20);
        board.GenerateRewards(5);
        
        Game game = new Game(new List<Player> { player1, player2, player3 }, board, playerTypes);

        game.OnSpecialTile += player => Console.WriteLine($"{player.Name} trafil na pole specjalne");
        
        game.StartGame();
    }
}