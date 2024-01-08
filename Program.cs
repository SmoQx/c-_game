using System;
using System.Threading;


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
        Obstacle obstacle = new Obstacle(10, 44, 2, 5);

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
                    obstacle.Render('*');
                    player.Render('P');
                    // Set the cursor position for the player
                    Console.Write(counter);

                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey();
                        player.Move(keyInfo.Key, gameMap.Width, gameMap.Height, obstacle);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            gameState = Game_state.MainMenu;
                        }
                    }
                    else
                    {
                        player.Move(ConsoleKey.NoName, gameMap.Width, gameMap.Height, obstacle);
                    }
                    await Task.Delay(50); // Adjust the delay to control the speed of the game
                    counter++;
                    break;

                // ... (other cases)
            }
        }
    }
}
