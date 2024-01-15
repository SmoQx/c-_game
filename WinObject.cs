public class WinObject : GameObject
{
    public WinObject(int height, int width, bool winCon = true, bool gravity = false)
        : base ('@', height, width, gravity, winCon) { }
}
