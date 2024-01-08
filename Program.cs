using System;
using System.Threading;

class GameObject
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public char Symbol {get; set; }

    public GameObject(int x, int y, int width, int height, int weight)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Weight = weight;
    }

    public bool CollidesWith(GameObject other)
    {
        return X < other.X + other.Width
            && X + Width > other.X
            && Y < other.Y + other.Height
            && Y + Height > other.Y;
    }

    public void Render(char symbol)
    {
        Symbol = symbol;
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(X + i, Y - j);
                Console.Write(Symbol);
            }
        }
    }

}

class Player : GameObject
{
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }
    public bool IsJumping { get; set; }

    public Player(int x, int y)
        : base(x, y, 2, 2, 1)
    {
        X = x;
        Y = y;
        VelocityX = 1;
        VelocityY = 0;
        IsJumping = false;
    }

    public void Move(ConsoleKey key, int mapWidth, int mapHeight, GameObject other)
    {
        int newPlayerX = X;
        int newPlayerY = Y;
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                if (X - VelocityX >= 0)
                {
                    X -= VelocityX;
                }
                break;

            case ConsoleKey.RightArrow:
                if (X + VelocityX < mapWidth)
                {
                    X += VelocityX;
                }
                break;

            case ConsoleKey.Spacebar:
                if (!IsJumping)
                {
                    Jump();
                }
                break;

            default:
                break;
        }

        if (CollidesWith(other))
        {
            // Update player position only if there is no collision
            X = newPlayerX;
            if (VelocityY > 0)
            {
                VelocityY = 0;
            }
            Y = newPlayerY;
        }

        ApplyGravity(mapHeight);
    }

    private void Jump()
    {
        IsJumping = true;
        VelocityY = 4; // Adjust the jump height
    }

    public void ApplyGravity(int mapHeight)
    {
        if (IsJumping)
        {
            // Only decrease Y during a jump
            if (Y - VelocityY >= 0)
            {
                Y -= VelocityY;
                VelocityY--; // Simulate gravity by decreasing the velocity
                Console.SetCursorPosition(0, 0);
                Console.Write("####");
            }
            else
            {
                Y = 0;
                IsJumping = false;
            }
        }
        if (Y > mapHeight - 1)
        { // Prevent going below the ground
            int newY = Y + VelocityY;
            if (newY < mapHeight - 1)
            {
                Y = mapHeight - 2;
                Console.SetCursorPosition(0, 51);
                Console.Write("{0}", newY);
            }
            else
            {
                Y = mapHeight - 2;
                VelocityY = 0; // Stop further descent when on the ground
            }
            IsJumping = false;
        }
    }
}

class Obsticle : GameObject
{
    public Obsticle(int x, int y, int height, int width)
        : base(x, y, height, width, 1)
    {
        Width = width;
        Height = height;
        X = x;
        Y = y;
    }

}

class Map
{
    public int Width { get; set; }
    public int Height { get; set; }

    // Properties for ground, walls, and ceiling
    public int GroundHeight { get; set; }
    public int WallWidth { get; set; }
    public int CeilingHeight { get; set; }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;

        // Set default properties for ground, walls, and ceiling
        GroundHeight = 2;
        WallWidth = 1;
        CeilingHeight = 2;
    }

    public void Render()
    {
        // Implement the map rendering logic here, including ground, walls, and ceiling
        for (int i = 0; i < GroundHeight; i++)
        {
            Console.SetCursorPosition(0, Height - i - 1);
            for (int j = 0; j < Width; j++)
            {
                Console.Write("-");
            }
        }

        for (int i = 0; i < WallWidth; i++)
        {
            for (int j = 0; j < Height - GroundHeight - CeilingHeight; j++)
            {
                Console.SetCursorPosition(i, j);
                Console.Write("|");
                Console.SetCursorPosition(Width - i - 1, j);
                Console.Write("|");
            }
        }

        for (int i = 0; i < CeilingHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            for (int j = 0; j < Width; j++)
            {
                Console.Write("-");
            }
        }
    }
}

public enum Game_state
{
    inGame,
    MainMenu,
    Pause,
    GameOver,
    Closing
}

class Program
{
    static async Task Main()
    {
        Map gameMap = new Map(100, 50);
        Player player = new Player(gameMap.Width / 2, gameMap.Height - gameMap.GroundHeight);
        Obsticle obsticle = new Obsticle(10, 44, 2, 5);

        Game_state gameState = Game_state.MainMenu; // Initial game state

        ConsoleKeyInfo keyInfo;
        Int32 counter = 0;
        while (gameState != Game_state.Closing)
        {
            Console.Clear();

            // Handle different game states
            switch (gameState)
            {
                case Game_state.MainMenu:
                    Console.WriteLine("Main Menu");
                    // Implement main menu logic and rendering here
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        gameState = Game_state.inGame;
                        Console.WriteLine("Now Playing {0}", gameState);
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        gameState = Game_state.Closing;
                    }
                    break;

                case Game_state.inGame:
                    // Render the map
                    gameMap.Render();
                    obsticle.Render('*');
                    player.Render('P');
                    // Set the cursor position for the player
                    Console.Write(counter);

                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey();
                        player.Move(keyInfo.Key, gameMap.Width, gameMap.Height, obsticle);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            gameState = Game_state.MainMenu;
                        }
                    }
                    else
                    {
                        player.Move(ConsoleKey.NoName, gameMap.Width, gameMap.Height, obsticle);
                    }
                    await Task.Delay(50); // Adjust the delay to control the speed of the game
                    counter++;
                    break;

                // ... (other cases)
            }
        }
    }
}
