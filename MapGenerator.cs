public class MapGenerator {
    private List<Element> Elements { get; set; }
    private int maxX { get; }
    private int maxY { get; }

    public MapGenerator(List<Element> elements, int gamesizeX, int gamesizeY)
    {
        maxX = gamesizeX;
        maxY = gamesizeY;
        Elements = elements;    
    }

    public List<Element> Generate(int howMany)
    {
        Random random1 = new Random();
        Random random2 = new Random();
        Random random3 = new Random();
        Random randomX = new Random();
        Random randomY = new Random();

        bool gravity = false;

        for (int i = 0; i < howMany; i++)
        {
            int randomInRangeSizeX = random3.Next(1, 5);
            int randomInRangeSizeY = random1.Next(1, 10);
            int x = randomX.Next(1, maxX - randomInRangeSizeX);
            int y = randomY.Next(1, maxY - randomInRangeSizeY);
            int randomGravity = random2.Next(1, 100);
            if (randomGravity > 75)
                gravity = true;
            else
                gravity = false;
            int newx = x ;
            int newy = y ;
            Elements.Add(new Element(new Obstacle(randomInRangeSizeX, randomInRangeSizeY, gravity: gravity), new Position(newx, newy)));
        }
        //foreach (var element in Elements)
        //{ 
        //    Console.Write(element);
        //} 
        return Elements;
    }
}
