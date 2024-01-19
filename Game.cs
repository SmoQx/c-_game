public class Game
{
    private List<Element> elements;
    private Player player;
    private Position playerPosition;
    private GameState gameState;
    private int gamesizeX;
    private int gamesizeY;
    private Map map;
    private MapGenerator mapGenerator;

    public Game(int width, int height)
    {
        gamesizeX = width;
        gamesizeY = height;
        elements = new List<Element>();
        map = new Map(gamesizeX, gamesizeY);
        mapGenerator = new MapGenerator(elements);
        player = new Player();
        playerPosition = new Position(width / 2, gamesizeY - player.Height);
        elements.Add(new Element(player, playerPosition));
        elements.Add(new Element(new WinObject(1, 1, winCon: true), new Position(19, 0 + 5)));
        elements.Add(
            new Element(
                new Obstacle(5, 2, gravity: true),
                new Position(gamesizeX - 10, gamesizeY - 17)
            )
        );
        elements.Add(new Element(new Obstacle(4, 2), new Position(32, gamesizeY - 25)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(39, 7)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(42, 12)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(37, 14)));
        elements.Add(new Element(new Obstacle(8, 2), new Position(23, 3)));
        elements.Add(
            new Element(new Obstacle(5, 2), new Position(gamesizeX / 2 + 3, gamesizeY - 4))
        );
        elements.Add(
            new Element(new Obstacle(2, 2, gravity: true), new Position(gamesizeX / 2, 13))
        );
        elements.Add(
            new Element(
                new Obstacle(2, 10, gravity: true),
                new Position(gamesizeX - 10, gamesizeY - 15)
            )
        );
        elements.Add(new Element(new Obstacle(5, 2), new Position(9, gamesizeY - 28)));
        elements.Add(new Element(new Obstacle(4, 2), new Position(12, gamesizeY - 23)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(9, gamesizeY - 18)));
        elements.Add(new Element(new Obstacle(1, 2), new Position(12, gamesizeY - 13)));
        elements.Add(new Element(new Obstacle(3, 2), new Position(9, gamesizeY - 8)));
        elements.Add(new Element(new Obstacle(2, 2), new Position(12, gamesizeY - 3)));
        elements.Add(new Element(new Obstacle(2, 2, gravity: true), new Position(15, 8)));
        elements.AddRange(mapGenerator.Generate(5));

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

                    Console.WriteLine("Press ENTER to play");
                    Console.WriteLine("Press esc to exit");
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
                    map.Render();
                    var key = ConsoleKey.NoName;
                    if (Console.KeyAvailable)
                    {
                        key = Console.ReadKey(true).Key;

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
                                if (player.IsJumping == false)
                                    Jump(player.VelocityY = 2);
                                player.IsJumping = true;
                                break;
                        }
                    }
                    if (Win(new List<int> { playerPosition.X, playerPosition.Y }, elements[0]))
                    {
                        gameState = GameState.GameOver;
                    }
                    Console.SetCursorPosition(gamesizeX, 0);
                    Console.WriteLine("Velocity Y" + player.VelocityY + ",");
                    Console.SetCursorPosition(gamesizeX, 1);
                    Console.WriteLine("Is Jumping" + player.IsJumping + ",");
                    Console.SetCursorPosition(gamesizeX, 2);
                    Console.WriteLine("player height" + player.Height + ",");
                    Console.SetCursorPosition(gamesizeX, 3);
                    Console.WriteLine("Pos Y + height" + playerPosition.Y + ",");
                    Console.SetCursorPosition(gamesizeX, 4);
                    Console.WriteLine("Pos X" + playerPosition.X + ",");
                    Console.SetCursorPosition(gamesizeX, 5);
                    Console.WriteLine("Y size" + gamesizeY + ",");
                    Gravity();
                    await Task.Delay(16); // Adjust the delay to control the speed of the game

                    break;

                case GameState.GameOver:
                    Console.WriteLine("You win!!");
                    Console.WriteLine("Press ENTER to continue");
                    Console.WriteLine("Press esc to exit");
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Escape:
                            gameState = GameState.Closing;
                            break;
                        case ConsoleKey.Enter:
                            playerPosition.X = gamesizeX / 2 - 2;
                            playerPosition.Y = gamesizeY - player.Height;
                            gameState = GameState.MainMenu;
                            break;
                    }

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

        for (int i = 0; i < y; i++)
        {
            if (CollidesWith(new List<int> { playerPosition.X, playerPosition.Y - i }, elements[0]))
            {
                veloY = i - 1;
                Console.Write(veloY);
                break;
            }
            else
            {
                continue;
            }
        }

        int newY = playerPosition.Y - veloY;

        if (
            !CollidesWith(new List<int> { playerPosition.X, playerPosition.Y - veloY }, elements[0])
        )
        {
            if (newY < gamesizeY && newY > 0)
            {
                playerPosition.Y = newY;
            }
        }
    }

    private void Gravity()
    {
        int player_velo_y = player.VelocityY;

        if (player.IsJumping == true && playerPosition.Y > 0)
        {
            if (player.VelocityY > 0)
            {
                player.VelocityY--;
                Jump(player.VelocityY);
            }
            else if (player.IsJumping == true && player.VelocityY <= 0)
                if (
                    playerPosition.Y + player.Height < gamesizeY
                    && !CollidesWith(
                        new List<int> { elements[0].Pos.X, elements[0].Pos.Y + 1 },
                        elements[0]
                    )
                )
                    playerPosition.Y++;
        }
        if (
            player.IsJumping == false
            && playerPosition.Y + player.Height < gamesizeY
            && !CollidesWith(
                new List<int> { elements[0].Pos.X, elements[0].Pos.Y + 1 },
                elements[0]
            )
            && !(
                CollidesWith(
                    new List<int> { elements[0].Pos.X - 1, elements[0].Pos.Y },
                    elements[0]
                )
                || CollidesWith(
                    new List<int> { elements[0].Pos.X + 1, elements[0].Pos.Y },
                    elements[0]
                )
            )
        )
        {
            playerPosition.Y++;
        }
        if (
            (
                CollidesWith(
                    new List<int> { elements[0].Pos.X, elements[0].Pos.Y + 1 },
                    elements[0]
                )
                || CollidesWith(
                    new List<int> { elements[0].Pos.X + 1, elements[0].Pos.Y },
                    elements[0]
                )
                || CollidesWith(
                    new List<int> { elements[0].Pos.X - 1, elements[0].Pos.Y },
                    elements[0]
                )
            )
            || elements[0].Pos.Y + elements[0].Object.Height == gamesizeY
        )
        {
            player.IsJumping = false;
        }

        foreach (var element in elements)
        {
            if (element != elements[0])
            {
                if (
                    element.Object.Gravity
                    && CollidesWith(new List<int> { element.Pos.X, element.Pos.Y + 1 }, element)
                )
                    continue;
                else if (
                    element.Object.Gravity
                    && element.Pos.Y + element.Object.Height < gamesizeY
                )
                {
                    element.Pos.Y = element.Pos.Y + 1;
                }
            }
        }
        Thread.Sleep(24);
    }

    private bool CollidesWith(List<int> pos, Element currentElement)
    {
        foreach (var otherElement in elements)
        {
            if (otherElement != currentElement && otherElement.Object.WinCon == false)
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

    private bool Win(List<int> pos, Element currentElement)
    {
        foreach (var otherElement in elements)
        {
            if (otherElement != currentElement && otherElement.Object.WinCon == true)
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
        return false;
    }
}
