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
