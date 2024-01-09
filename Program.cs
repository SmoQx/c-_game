using System;
using System.Threading;


class Program
{
    static async Task Main()
    {
        Map gameMap = new Map(100, 50);
        Player player = new Player(gameMap.Width / 2, gameMap.Height - gameMap.GroundHeight);
        Obstacle obstacle = new Obstacle(10, 44, 2, 5);

        GameState gameState = GameState.MainMenu; // Initial game state

        ConsoleKeyInfo keyInfo;
        Int32 counter = 0;
        while (gameState != GameState.Closing)
        {
            Console.Clear();

            // Handle different game states
            switch (gameState)
            {
                case GameState.MainMenu:
                    Console.WriteLine("Main Menu");
                    // Implement main menu logic and rendering here
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        gameState = GameState.inGame;
                        Console.WriteLine("Now Playing {0}", gameState);
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        gameState = GameState.Closing;
                    }
                    break;

                case GameState.inGame:
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
                            gameState = GameState.MainMenu;
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
