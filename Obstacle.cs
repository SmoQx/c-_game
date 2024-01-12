public class Obstacle : GameObject
{
    public Obstacle(int height, int width, bool gravity = false)
        : base('*', height, width, gravity) { }
}
