class Program
{
    static async Task Main()
    {
        var g = new Game(50, 30);
        await g.Run();
    }
}
