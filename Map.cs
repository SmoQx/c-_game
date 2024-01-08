class Map
{
    public int Width { get; set; }
    public int Height { get; set; }

    // Properties for ground, walls, and ceiling
    public int GroundHeight { get; set; }
    public int WallWidth { get; set; }
    public int CeilingHeight { get; set; }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;

        // Set default properties for ground, walls, and ceiling
        GroundHeight = 2;
        WallWidth = 1;
        CeilingHeight = 2;
    }

    public void Render()
    {
        // Implement the map rendering logic here, including ground, walls, and ceiling
        for (int i = 0; i < GroundHeight; i++)
        {
            Console.SetCursorPosition(0, Height - i - 1);
            for (int j = 0; j < Width; j++)
            {
                Console.Write("-");
            }
        }

        for (int i = 0; i < WallWidth; i++)
        {
            for (int j = 0; j < Height - GroundHeight - CeilingHeight; j++)
            {
                Console.SetCursorPosition(i, j);
                Console.Write("|");
                Console.SetCursorPosition(Width - i - 1, j);
                Console.Write("|");
            }
        }

        for (int i = 0; i < CeilingHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            for (int j = 0; j < Width; j++)
            {
                Console.Write("-");
            }
        }
    }
}
