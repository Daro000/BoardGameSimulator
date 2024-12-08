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

class Game
{
    public List<Player> Players { get; set; }
    public Board Board { get; set; }
    public int CurrentTurn { get; set; }

    public Game(List<Player> players, Board board)
    {
        Players = players;
        Board = board;
        CurrentTurn = 0;
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
    
}
