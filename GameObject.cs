class GameObject
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public char Symbol { get; set; }

    public GameObject(int x, int y, int width, int height, int weight)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Weight = weight;
    }

    public bool CollidesWith(GameObject other)
    {
        return X < other.X + other.Width
            && X + Width > other.X
            && Y < other.Y + other.Height
            && Y + Height > other.Y;
    }

    public void Render(char symbol)
    {
        Symbol = symbol;
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(X + i, Y - j);
                Console.Write(Symbol);
            }
        }
    }

}


