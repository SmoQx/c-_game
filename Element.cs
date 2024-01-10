public class Element
{
    public GameObject Object { get; }
    public Position Pos { get; set; }

    public Element(GameObject obj, Position pos)
    {
        this.Object = obj;
        this.Pos = pos;
    }

    public void Render()
    {
        Console.SetCursorPosition(this.Pos.X, this.Pos.Y);
        Object.Render();
    }
}
