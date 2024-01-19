public class MapGenerator {
    private List<Element> Elements { get; set; }

    public MapGenerator(List<Element> elements)
    {
        Elements = elements;    
    }

    public List<Element> Generate(int howMany)
    {
        int width = 2;
        int height = 2;
        bool gravity = false;
        int x = 2;
        int y = 2;

        for (int i = 0; i < howMany; i++)
        {
            int newx = i + x;
            int newy = i^2 + i * y;
            Elements.Add(new Element(new Obstacle(width, height, gravity: gravity), new Position(newx, newy)));
        }
        //foreach (var element in Elements)
        //{ 
        //    Console.Write(element);
        //} 
        return Elements;
    }
}
