using System;
using System.Threading;

class Player {
    public int X { get; set; }
    public int Y { get; set; }
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }
    public bool IsJumping { get; set; }

    public Player(int x, int y) {
        X = x;
        Y = y;
        VelocityX = 1;
        VelocityY = 0;
        IsJumping = false;
    }

    public void Move(ConsoleKey key, int mapWidth, int mapHeight) {
        switch (key) {
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

        ApplyGravity(mapHeight);
    }

    private void Jump() {
        IsJumping = true;
        VelocityY = 4; // Adjust the jump height
    }

    public void ApplyGravity(int mapHeight) {
        if (IsJumping) {
            // Only decrease Y during a jump
            if (Y - VelocityY >= 0) {
                Y -= VelocityY;
                VelocityY--; // Simulate gravity by decreasing the velocity
                Console.SetCursorPosition(0,0);
                Console.Write("####");
            }
            else {
                Y = 0;
                IsJumping = false;
            }

        }
        if (Y > mapHeight - 1) { // Prevent going below the ground
            int newY = Y + VelocityY;
            if (newY < mapHeight - 1) {
                Y = mapHeight-2;
                Console.SetCursorPosition(0,51);
                Console.Write("{0}", newY);
            }
            else {
                Y = mapHeight - 2;
                VelocityY = 0; // Stop further descent when on the ground
            }
            IsJumping = false;
        }
    }
}

class Obsticle  {
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Obsticle(int x, int y, int height, int width){
        Width = width;
        Height = height;
        X = x;
        Y = y;
    }
    public void Render(){
        for (int j = 0; j < Width; j++){ 
            for (int i = 0; i < Height; i++){
                Console.SetCursorPosition(X + i, Y + j);
                Console.Write("*");
            }
        }
    }
}

class Map {
    public int Width { get; set; }
    public int Height { get; set; }

    // Properties for ground, walls, and ceiling
    public int GroundHeight { get; set; }
    public int WallWidth { get; set; }
    public int CeilingHeight { get; set; }

    public Map(int width, int height) {
        Width = width;
        Height = height;

        // Set default properties for ground, walls, and ceiling
        GroundHeight = 2;
        WallWidth = 1;
        CeilingHeight = 2;
    }

    public void Render() {
        // Implement the map rendering logic here, including ground, walls, and ceiling
        for (int i = 0; i < GroundHeight; i++) {
            Console.SetCursorPosition(0, Height - i - 1);
            for (int j = 0; j < Width; j++) {
                Console.Write("-");
            }
        }

        for (int i = 0; i < WallWidth; i++) {
            for (int j = 0; j < Height - GroundHeight - CeilingHeight; j++) {
                Console.SetCursorPosition(i, j);
                Console.Write("|");
                Console.SetCursorPosition(Width - i - 1, j);
                Console.Write("|");
            }
        }

        for (int i = 0; i < CeilingHeight; i++) {
            Console.SetCursorPosition(0, i);
            for (int j = 0; j < Width; j++) {
                Console.Write("-");
            }
        }
    }
}

public enum Game_state {
    inGame,
    MainMenu,
    Pause,
    GameOver,
    Closing
}

class Program {
    static async Task Main() {
        Map gameMap = new Map(100, 50);
        Player player = new Player(gameMap.Width / 2, gameMap.Height - gameMap.GroundHeight);
        Obsticle obsticle = new Obsticle(10, 20, 1, 5);

        Game_state gameState = Game_state.MainMenu; // Initial game state

        ConsoleKeyInfo keyInfo;

        while (gameState != Game_state.Closing) {
            Console.Clear();

            // Handle different game states
            switch (gameState) {
                case Game_state.MainMenu:
                    Console.WriteLine("Main Menu");
                    // Implement main menu logic and rendering here
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Enter) {
                        gameState = Game_state.inGame;
                        Console.WriteLine("Now Playing {0}", gameState);
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape) {
                        gameState = Game_state.Closing;
                    }
                    break;

                case Game_state.inGame:
                    // Render the map
                    gameMap.Render();
                    obsticle.Render();

                    // Set the cursor position for the player
                    Console.SetCursorPosition(player.X, player.Y);
                    Console.Write("P{0},{1},{2}", player.X, player.Y, player.IsJumping);
                    if (Console.KeyAvailable){
                        keyInfo = Console.ReadKey();
//                        if (keyInfo.Key == ConsoleKey.NoName){
//                            Console.Clear();
//                        }
                        player.Move(keyInfo.Key, gameMap.Width, gameMap.Height);
                        if (keyInfo.Key == ConsoleKey.Escape) {
                            gameState = Game_state.MainMenu;
                        }
                    }
                    await Task.Delay(50); // Adjust the delay to control the speed of the game
                    break;

                // ... (other cases)

            }
        } 
    }
}
