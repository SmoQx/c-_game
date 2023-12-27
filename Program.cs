using System;


class Player
{
    public int X { get; set; }
    public int Y { get; set; }
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }
    public bool IsJumping { get; set; }

    public Player(int x, int y)
    {
        X = x;
        Y = y;
        VelocityX = 1;
        VelocityY = 0;
        IsJumping = false;
    }

    public void Move(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                X -= VelocityX;
                break;

            case ConsoleKey.RightArrow:
                X += VelocityX;
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

        ApplyGravity();
    }

    private void Jump()
    {
        IsJumping = true;
        VelocityY = 1; // Adjust the jump height
    }

    private void ApplyGravity()
    {
        if (Y > 0) // Prevent going below the ground
        {
            Y -= VelocityY;
        }
        else
        {
            Y = 0;
            IsJumping = false;
        }
    }
}

class Ground
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Ground(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void Render()
    {
        // Implement the ground rendering logic here
    }
}

class Obstacle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Obstacle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool CollideWithPlayer(Player player)
    {
        // Implement the logic for checking collision with the player here
        return false; // Placeholder, replace with actual collision logic
    }

    public void Render()
    {
        // Implement the obstacle rendering logic here
    }
}

class Program
{
    static void Main()
    {
        Player player = new Player(10, 10);

        ConsoleKeyInfo keyInfo;

        do
        {
            Console.Clear();
            Console.SetCursorPosition(player.X, player.Y);
            Console.Write("P");

            keyInfo = Console.ReadKey();
            player.Move(keyInfo.Key);

            Thread.Sleep(5); // Add a short delay to control the speed of the game

        } while (keyInfo.Key != ConsoleKey.Escape);
    }
}

