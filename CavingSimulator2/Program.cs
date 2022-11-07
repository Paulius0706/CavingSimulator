namespace CavingSimulator2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720, "View"))
            {

                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run();
            }
        }
    }
}