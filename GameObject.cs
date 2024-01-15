public abstract class GameObject
{
    public int Width { get; set; }
    public int Height { get; set; }
    public char Symbol { get; }
    public bool Gravity { get; set; }
    public bool WinCon { get; }

    public GameObject(char symbol, int width, int height, bool gravity = false, bool winCon = false)
    {
        Symbol = symbol;
        Width = width;
        Height = height;
        Gravity = gravity;
        WinCon = winCon; 
    }

    /*
    public bool CollidesWith(GameObject other)
    {
        return X < other.X + other.Width
            && X + Width > other.X
            && Y < other.Y + other.Height
            && Y + Height > other.Y;
    }
    */

    public virtual void Render()
    {
        var (x, y) = Console.GetCursorPosition();
        var width = new String(Enumerable.Repeat(Symbol, Width).ToArray());

        for (var i = 0; i < Height; i++)
        {
            if (WinCon == true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            Console.SetCursorPosition(x, y + i);
            Console.Write(width);
            Console.ResetColor();
        }

        Console.SetCursorPosition(x, y);
    }
}
