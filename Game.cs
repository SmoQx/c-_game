public class Game
{
    private List<Element> elements;
    private Player player;
    private Position playerPosition;
    private GameState gameState;

    public Game(int width, int height)
    {
        elements = new List<Element>();

        player = new Player();
        playerPosition = new Position(width / 2, height - player.Height);
        elements.Add(new Element(player, playerPosition));
        elements.Add(new Element(new Obstacle(2, 5), new Position(10, 13)));

        gameState = GameState.MainMenu;
    }

    public async Task Run()
    {
        while (gameState != GameState.Closing)
        {
            Console.Clear();

            // Handle different game states
            switch (gameState)
            {
                case GameState.MainMenu:
                    Console.WriteLine("Main Menu");

                    // Implement main menu logic and rendering here
                    var keyInfo = Console.ReadKey();

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            gameState = GameState.InGame;
                            Console.WriteLine("Now Playing {0}", gameState);
                            break;

                        case ConsoleKey.Escape:
                            gameState = GameState.Closing;
                            break;
                    }

                    break;

                case GameState.InGame:
                    foreach (var element in elements)
                    {
                        element.Render();
                    }
                    var key = ConsoleKey.NoName;
                    if (Console.KeyAvailable)
                        key = Console.ReadKey().Key;

                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            gameState = GameState.MainMenu;
                            break;
                        case ConsoleKey.LeftArrow:
                            Move(-1);
                            break;
                        case ConsoleKey.RightArrow:
                            Move(1);
                            break;
                        case ConsoleKey.Spacebar:
                            Jump(4);
                            break;

                    }
                    // player.Move(key, gameMap.Width, gameMap.Height, obstacle);

                    await Task.Delay(150); // Adjust the delay to control the speed of the game

                    break;

                default:
                    // ... (other cases)
                    break;
            }
        }
    }

    public void Move(int x)
    {
        int newX = elements[0].Pos.X + x;
            if (newX >= 0 && newX <= 100 - player.Width)
            {
                playerPosition.X = newX;
            }
    }

    private void Jump(int y)
    {
        int veloY = y;
        for (int i = 0; veloY >= i; veloY--){
            Console.Write(i);
            int newY = playerPosition.Y - veloY;
            if (newY <= 50){
                playerPosition.Y = newY;

            }
            Console.Write(playerPosition.Y);
        }
    }

    private int Gravity()
    {
        //todo
        return 0;
    }

    private void OnTheFloor()
    {
        //todo
    }
}
