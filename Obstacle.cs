class Obstacle : GameObject
{
    public Obstacle(int x, int y, int height, int width)
        : base(x, y, height, width, 1)
    {
        Width = width;
        Height = height;
        X = x;
        Y = y;
    }
}
