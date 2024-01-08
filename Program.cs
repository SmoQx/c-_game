class Program
{
    static async Task Main()
    {
        var g = new Game(50, 20);
        await g.Run();
    }
}
