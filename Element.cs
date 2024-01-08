public class Element
{
    public Element(GameObject obj, Position pos)
    {
        this.Object = obj;
        this.Pos = pos;
    }

    public GameObject Object { get; }
    public Position Pos { get; }

    public void Render()
    {
        Console.SetCursorPosition(Pos.X, Pos.Y);
        Object.Render();
    }
}

