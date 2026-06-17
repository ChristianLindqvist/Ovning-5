internal class Program
{
    private static void Main(string[] args)
    {
        Handler handler = new Handler();
        handler.SeedGarage();

        IUI ui = new UI(handler);
        ui.Run();
    }
}