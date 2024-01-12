public class Player : GameObject
{
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }
    public bool IsJumping { get; set; }

    public Player()
        : base('P', 2, 2)
    {
        VelocityX = 1;
        VelocityY = 1;
    }
}
