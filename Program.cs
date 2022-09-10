using System;

namespace GameEngine
{

    class Program
    {
        public static Window? GetWindow()
            => gameWindow;

        static Window? gameWindow;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing window");

            gameWindow = new Window("Game Engine", 1200, 900, true);
            gameWindow.Run();
        }
    }
}
