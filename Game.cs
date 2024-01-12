public class Game
{
    private List<Element> elements;
    private Player player;
    private Position playerPosition;
    private GameState gameState;
    private int gamesizeX;
    private int gamesizeY;
    private Map map;

    public Game(int width, int height)
    {
        gamesizeX = width;
        gamesizeY = height;
        elements = new List<Element>();
        map = new Map(gamesizeX, gamesizeY);
        player = new Player();
        playerPosition = new Position(width / 2, height - player.Height);
        elements.Add(new Element(player, playerPosition));
        elements.Add(new Element(new Obstacle(2, 5), new Position(10, 13)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(8, 13)));

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
                    //map.Render();
                    var key = ConsoleKey.NoName;
                    if (Console.KeyAvailable)
                        key = Console.ReadKey().Key;

                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            gameState = GameState.MainMenu;
                            break;
                        case ConsoleKey.LeftArrow:
                            Move(-player.VelocityX);
                            break;
                        case ConsoleKey.RightArrow:
                            Move(player.VelocityX);
                            break;
                        case ConsoleKey.Spacebar:
                            Jump(player.VelocityY);
                            break;
                        case ConsoleKey.DownArrow:
                            Jump(-player.VelocityY);
                            break;
                    }

                    await Task.Delay(10); // Adjust the delay to control the speed of the game

                    break;

                default:
                    // ... (other cases)
                    break;
            }
        }
    }

    private void Move(int x)
    {
        int newX = elements[0].Pos.X + x;
        int oldX = playerPosition.X;
        if (CollidesWith(new List<int>{playerPosition.X + x, playerPosition.Y}) != 3)
            
        if (newX >= 0 && newX <= gamesizeX - player.Width)
        {
            playerPosition.X = newX;
        }
    }

    private void Jump(int y)
    {
        int veloY = y;
        int newY = playerPosition.Y - veloY;
        if (CollidesWith(new List<int>{playerPosition.X, playerPosition.Y - y}) != 3)
        
        if (newY <= 50)
        {
            playerPosition.Y = newY;
        }
    }

    private int Gravity()
    {
        //todo
        return 0;
    }

    private int CollidesWith(List<int> pos)
    {
        var colidesX = false;
        var colidesY = false;
        var colides = 0;
        foreach (var element in elements)
        {
            if (element == elements[0])
                continue;
            if (
                pos[0] + player.Width > element.Pos.X
                && pos[0] < element.Pos.X + element.Object.Width
            )
            {
                colidesX = true;
            }
            if (
                pos[1] < element.Pos.Y + element.Object.Height
                && pos[1] + player.Height - 1 >= element.Pos.Y
            )
            {
                colidesY = true;
            }
            if (colidesY && colidesX)
            {
                Console.Write("colides");
                return 3;
            }
        }
        return colides;
    }
}
