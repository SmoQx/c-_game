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
        elements.Add(new Element(new Obstacle(2, 3, true), new Position(20, 14)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(8, 13)));
        elements.Add(new Element(new Obstacle(10, 2), new Position(10, 17)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(15, 8)));

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
                            Jump(player.VelocityY = 5);
                            break;
                        case ConsoleKey.DownArrow:
                            Jump(-player.VelocityY);
                            break;
                    }
                    Gravity();
                    await Task.Delay(50); // Adjust the delay to control the speed of the game

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
        if (!(CollidesWith(new List<int> { playerPosition.X + x, playerPosition.Y }, elements[0])))
            if (newX >= 0 && newX <= gamesizeX - player.Width)
            {
                playerPosition.X = newX;
            }
    }

    private void Jump(int y)
    {
        int veloY = y;
        for (int i = y; i >= 0; i--)
            if ((CollidesWith(new List<int> { playerPosition.X, playerPosition.Y - i }, elements[0])))
            {
                continue;
            }
            else
            {
                veloY = i;
                break;
            }
        Console.Write(veloY);
        int newY = playerPosition.Y - veloY;
        if (!(CollidesWith(new List<int> { playerPosition.X, playerPosition.Y - veloY }, elements[0])))
            if (newY <= 50)
            {
                playerPosition.Y = newY;
            }
    }

    private void Gravity()
    {
        foreach (var element in elements)
        {
            if (
                element.Object.Gravity
                && CollidesWith(new List<int> { element.Pos.X, element.Pos.Y + 1 }, element)
            )
                continue;
            else if (element.Object.Gravity && element.Pos.Y + element.Object.Height < gamesizeY)
            {
                element.Pos.Y = element.Pos.Y + 1;
            }
        }
    }

    private bool CollidesWith(List<int> pos, Element currentElement)
    {
        foreach (var otherElement in elements)
        {
            if (otherElement != currentElement)
            {
                if (
                    pos[0] + currentElement.Object.Width > otherElement.Pos.X
                    && pos[0] < otherElement.Pos.X + otherElement.Object.Width
                    && pos[1] < otherElement.Pos.Y + otherElement.Object.Height
                    && pos[1] + currentElement.Object.Height > otherElement.Pos.Y
                )
                {
                    return true; // Collision detected
                }
            }
        }
        return false; // No collision
    }
}
